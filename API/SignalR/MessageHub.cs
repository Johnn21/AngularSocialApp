using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PresenceTracker _tracker;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly IMapper _mapper;

        public MessageHub(IUnitOfWork unitOfWork, PresenceTracker tracker,
             IHubContext<PresenceHub> presenceHub, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tracker = tracker;
            _presenceHub = presenceHub;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var otherUserEntity = await _unitOfWork.UserRepository.GetUserByUsernameAsync(otherUser);
            var messages = await _unitOfWork.MessageRepository.GetMessagesBetweenUsers(Context.User.GetUserId(), otherUserEntity.Id);

            if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageFroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SenderIsTyping(SenderTypingDto senderTypingDto) 
        {
            var username = Context.User.GetUsername();

            if (username == senderTypingDto.ReceiverUsername)
                throw new HubException("You cannot type messages to yourself");

            var receiver = await _unitOfWork.UserRepository.GetMemberByUsernameAsync(senderTypingDto.ReceiverUsername);

            if (receiver == null) throw new HubException("Not found user");

            var groupName = GetGroupName(username, receiver.UserName);

            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == receiver.UserName))
            {
                var connections = await _tracker.GetConnectionsForUser(receiver.UserName);

                if (!string.IsNullOrEmpty(senderTypingDto.Content))
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("OtherUserIsTyping", true);
                }
                else 
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("OtherUserIsTyping", false);
                }
            }
        }

        public async Task SendMessage(SendMessageDto sendMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == sendMessageDto.ReceiverUsername)
                throw new HubException("You cannot send messages to yourself");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var receiver = await _unitOfWork.UserRepository.GetUserByUsernameAsync(sendMessageDto.ReceiverUsername);

            if (receiver == null) throw new HubException("Not found user");

            var message = new Message
            {
                SenderUser = sender,
                SenderUserId = sender.Id,
                ReceiverUser = receiver,
                ReceiverUserId = receiver.Id,
                Content = sendMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, receiver.UserName);

            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == receiver.UserName))
            {
                message.DateMessageRead = DateTime.Now;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(receiver.UserName);
                if (connections != null) 
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new {username = sender.UserName});
                }
            }

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageFroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
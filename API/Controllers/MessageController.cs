using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<MessageDto>> SendMessage([FromBody] SendMessageDto sendMessageDto)
        {
            var receiveruser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(sendMessageDto.ReceiverUsername);

            if (receiveruser == null) return BadRequest("This user does not exist");

            if (receiveruser.Id == User.GetUserId()) return BadRequest("You can not send messages to yourself");

            bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), receiveruser.Id);

            if (!friendship) return BadRequest("You must be a friend with this user in order to send a message");

            var message = new Message
            {
                Content = sendMessageDto.Content,
                SenderUserId = User.GetUserId(),
                ReceiverUserId = receiveruser.Id
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            var messageDto = _mapper.Map<MessageDto>(message);
            messageDto.ReceiverUsername = receiveruser.UserName;
            messageDto.SenderUsername = User.GetUsername();

            if (await _unitOfWork.Complete()) return Ok(messageDto);

            return BadRequest("Failed to send message");
        }

        [HttpGet("get-messages-between-users/{username}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessagesBetweenUsers(string username)
        {
            var receiveruser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (receiveruser == null) return BadRequest("This user does not exist");

            bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), receiveruser.Id);

            if (!friendship) return BadRequest("You must be a friend with this user in order to have messages");

            var messages = await _unitOfWork.MessageRepository.GetMessagesBetweenUsers(User.GetUserId(), receiveruser.Id);

            return Ok(messages);            
        }

    }
}
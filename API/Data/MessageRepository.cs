using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public async Task<List<MessageDto>> GetMessagesBetweenUsers(string senderUserId, string receiverUserId)
        {
            return await _context.Messages
                        .Where(x => (x.SenderUserId == senderUserId && x.ReceiverUserId == receiverUserId) 
                                || (x.SenderUserId == receiverUserId && x.ReceiverUserId == senderUserId))
                        .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)    
                        .OrderBy(x => x.DateMessageSend)
                        .ToListAsync();
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                    .Include(c => c.Connections)
                    .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                    .FirstOrDefaultAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(c => c.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }
    }
}
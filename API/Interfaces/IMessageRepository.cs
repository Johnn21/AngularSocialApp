using API.Data;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Task<List<MessageDto>> GetMessagesBetweenUsers(string senderUserId, string receiverUserId);
        Task<Group> GetGroupForConnection(string connectionId);
        void RemoveConnection(Connection connection);
        Task<Group> GetMessageGroup(string groupName);
        void AddGroup(Group group);
    }
}
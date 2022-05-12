using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Task<List<MessageDto>> GetMessagesBetweenUsers(string senderUserId, string receiverUserId);
    }
}
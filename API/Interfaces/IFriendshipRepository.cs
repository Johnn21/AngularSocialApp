using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<bool> CheckFriendship(string currentUserId, string friendUserId);
        void AddFriendship(Friendship friendship);
        Task<List<FriendDto>> GetFriendsByCurrentUserId(string currentUserId);
    }
}
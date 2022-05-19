using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<bool> CheckFriendship(string currentUserId, string friendUserId);
        void AddFriendship(Friendship friendship);
        Task<List<FriendDto>> GetFriendsByUserId(string userId);
        Task<PagedList<FriendDto>> GetFriendsByUserIdPaginated(PaginationParams paginationParams, string userId);
    }
}
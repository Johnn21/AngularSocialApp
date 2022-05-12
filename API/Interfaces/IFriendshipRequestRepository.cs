using API.Entities;

namespace API.Interfaces
{
    public interface IFriendshipRequestRepository
    {
        Task SendFriendshipRequest(FriendshipRequest friendshipRequest);
        Task<bool> CheckIfRequestFriendshipAlreadyExists(FriendshipRequest friendshipRequest);
        Task<AppUser> GetUserWithFriendShipRequests(string username);
        Task<AppUser> GetUserWithFriendShipReceived(string username);
        Task DeleteFriendRequest(string currentUserId, string friendUserId);
    }
}
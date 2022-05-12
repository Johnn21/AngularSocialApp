using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FriendshipRequestRepository : IFriendshipRequestRepository
    {
        private readonly StoreContext _context;

        public FriendshipRequestRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task SendFriendshipRequest(FriendshipRequest friendshipRequest)
        {
           await _context.FriendshipRequests.AddAsync(friendshipRequest);
        }

        public async Task<bool> CheckIfRequestFriendshipAlreadyExists(FriendshipRequest friendshipRequest) 
        {
            var result = await _context.FriendshipRequests.SingleOrDefaultAsync(x => x.SourceUserId == friendshipRequest.SourceUserId 
                        && x.DestinationUserId == friendshipRequest.DestinationUserId);
            
            if (result == null) return false;

            return true;
        }

        public async Task<AppUser> GetUserWithFriendShipRequests(string username)
        {
            return await _context.Users
                .Include(fr => fr.FriendshipRequestsSended)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetUserWithFriendShipReceived(string username)
        {
            return await _context.Users
                .Include(fr => fr.FriendshipRequestsReceived)
                .ThenInclude(u => u.SourceUser)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

         public async Task DeleteFriendRequest(string currentUserId, string friendUserId) 
        {
            var result = await _context.FriendshipRequests.SingleOrDefaultAsync(x => x.SourceUserId == currentUserId
                        && x.DestinationUserId == friendUserId);
            
            if (result == null) 
            {
                result = await _context.FriendshipRequests.SingleOrDefaultAsync(x => x.SourceUserId == friendUserId
                    && x.DestinationUserId == currentUserId);
            }

            _context.FriendshipRequests.Remove(result);
        }
    }
}
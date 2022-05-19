using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public FriendshipRepository(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckFriendship(string currentUserId, string friendUserId)
        {
            var result = await _context.Friendships
                .FirstOrDefaultAsync(x => x.CurrentUserId == currentUserId && x.FriendUserId == friendUserId);

            if (result == null) return false;

            return true;
        }

        public void AddFriendship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
        }

        public async Task<List<FriendDto>> GetFriendsByUserId(string userId)
        {
            var friends = await _context.Friendships.Where(x => x.CurrentUserId == userId).ToListAsync();

            List<FriendDto> friendsList = new List<FriendDto>();

            if (friends != null) 
            {
                foreach(var friend in friends)
                {
                    var member = await  _context.Users
                                    .Where(x => x.Id == friend.FriendUserId)
                                    .ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
                                    .SingleOrDefaultAsync();

                    friendsList.Add(member);                    
                }
            }

            return friendsList;
        }

        public async Task<PagedList<FriendDto>> GetFriendsByUserIdPaginated(PaginationParams paginationParams, string userId)
        {
            var count = await _context.Friendships
                .AsQueryable()
                .Where(x => x.CurrentUserId == userId)
                .CountAsync();

            var friends = await _context.Friendships
                .Where(x => x.CurrentUserId == userId)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            List<FriendDto> friendsList = new List<FriendDto>();

            if (friends != null) 
            {
                foreach(var friend in friends)
                {
                    var member = await  _context.Users
                                    .Where(x => x.Id == friend.FriendUserId)
                                    .ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
                                    .SingleOrDefaultAsync();

                    friendsList.Add(member);                    
                }
            }

            return new PagedList<FriendDto>(friendsList, count, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
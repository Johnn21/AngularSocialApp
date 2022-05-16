using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public PostRepository(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public void AddPost(Post post)
        {
            _context.Post.Add(post);
        }

        public async Task<Post> GetLastPost()
        {
            return await _context.Post
                .OrderByDescending(x => x.DateCreated)
                .Include(p => p.PhotoPost)
                .FirstAsync();
        }

        public async Task<List<PostDto>> GetFriendsPost(string currentUserId, int skipPosts, int takePosts)
        {
            var friendsIds = await _context.Friendships
                .Where(x => x.CurrentUserId == currentUserId)
                .ToListAsync();
            
            List<PostDto> posts = new List<PostDto>();

            if (friendsIds.Any()) 
            {               
                foreach(var friendId in friendsIds)
                {
                    var friendPosts = await _context.Post
                        .Where(x => x.AppUserId.Equals(friendId.FriendUserId))
                        .Include(p => p.PhotoPost)
                        .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                        .Skip(skipPosts * takePosts)
                        .Take(takePosts)
                        .OrderByDescending(x => x.DateCreated)
                        .ToListAsync();
                        
                    if (friendPosts.Any()) posts.AddRange(friendPosts);
                }
            }

            return posts;
        }
    }
}
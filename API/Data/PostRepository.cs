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
                        .Include(l => l.Likes)
                        .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                        .Skip(skipPosts * takePosts)
                        .Take(takePosts)
                        .OrderByDescending(x => x.DateCreated)
                        .ToListAsync();

                    foreach(var friendPost in friendPosts.ToList()) 
                    {
                        foreach(var like in friendPost.Likes.ToList())
                        {
                            if (like.AppUserId != currentUserId)
                            {
                                friendPost.Likes.Remove(like);
                            }
                        }
                    }
                        
                    if (friendPosts.Any()) posts.AddRange(friendPosts);
                }
            }

            return posts;
        }

        public async Task<Post> GetPostByIdWithLikes(int postId)
        {
            return await _context.Post
                .Include(l => l.Likes)
                .SingleAsync(x => x.Id == postId);
        }

        public void Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
        }

        public async Task<Post> GetPostByIdWithPostComments(int postId)
        {
            return await _context.Post
                .Include(pc => pc.PostComments)
                .SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<PostCommentDto>> GetPostCommentsByPostId(int postId)
        {
            return await _context.PostComments
                .Include(u => u.AppUser)
                .ThenInclude(p => p.Photos)
                .Where(p => p.PostId == postId)
                .ProjectTo<PostCommentDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(o => o.DateCreated)
                .ToListAsync();
        }
    }
}
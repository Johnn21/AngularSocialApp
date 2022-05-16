using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly StoreContext _context;

        public PostRepository(StoreContext context)
        {
            _context = context;
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
    }
}
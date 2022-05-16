using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void AddPost(Post post);
        Task<Post> GetLastPost();
        Task<List<PostDto>> GetFriendsPost(string currentUserId, int skipPosts, int takePosts);
    }
}
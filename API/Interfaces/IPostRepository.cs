using API.Entities;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void AddPost(Post post);
        Task<Post> GetLastPost();
    }
}
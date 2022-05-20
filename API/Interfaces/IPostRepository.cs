using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void AddPost(Post post);
        Task<Post> GetLastPost();
        Task<List<PostDto>> GetFriendsPost(string currentUserId, int skipPosts, int takePosts);
        Task<Post> GetPostByIdWithLikes(int postId);
        void Update(Post post);
        Task<Post> GetPostByIdWithPostComments(int postId);
        Task<List<PostCommentDto>> GetPostCommentsByPostId(int postId, int skipPostComments, int takePostComments);
    }
}
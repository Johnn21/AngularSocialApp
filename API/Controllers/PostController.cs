using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public PostController(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }
        
        [HttpPost("add-post")]
        public async Task<ActionResult<bool>> AddPost([FromBody] AddPostInfoDto addPostInfoDto)
        {
            var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var post = new Post
            {
                Title = addPostInfoDto.Title,
                Description = addPostInfoDto.Description,
                DateCreated = DateTime.Now,
                AppUserId = currentUser.Id,
                AppUser = currentUser,
                HasPhoto = false
            };

            _unitOfWork.PostRepository.AddPost(post);

            if (await _unitOfWork.Complete()) return Ok(true);

            return BadRequest("Failed to add a post");
        }

        [HttpPost("add-photo-to-post")]
        public async Task<ActionResult<bool>> AddPhotoToPost(IFormFile file)
        {
            var lastPost = await _unitOfWork.PostRepository.GetLastPost();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new PhotoPost
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            lastPost.PhotoPost = photo;
            lastPost.HasPhoto = true;

            if (await _unitOfWork.Complete()) return Ok(true);

            return BadRequest("Failed to add a photo to your post");
        }

        [HttpGet("get-friends-posts")]
        public async Task<ActionResult<List<PostDto>>> GetFriendsPost([FromQuery]int skipPosts)
        {
            var posts = await _unitOfWork.PostRepository.GetFriendsPost(User.GetUserId(), skipPosts, Constants.PostsParams.TakePosts);

            return posts;
        }

        [HttpPost("add-dislike-to-post/{postId}")]
        public async Task<ActionResult<PostDto>> AddDislikeToPost(int postId)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdWithLikes(postId);

            if (post == null) return BadRequest("This post does not exist");

            var postLike = post.Likes
                .Where(x => x.AppUserId == User.GetUserId())
                .SingleOrDefault();

            if (postLike != null) 
            {
                if (!postLike.Liked) 
                {
                    post.Likes.Remove(postLike);
                    post.DislikesCount--;
                }

                if (postLike.Liked) 
                {
                    postLike.Liked = false;
                    post.LikesCount--;
                    post.DislikesCount++;
                }

                _unitOfWork.PostRepository.Update(post);
            }
            else
            {
                var like = new Like
                {
                    Liked = false,
                    AppUserId = User.GetUserId(),
                    PostId = post.Id,
                    // Post = post
                };

                post.DislikesCount++;

                post.Likes.Add(like);
            }

            if (await _unitOfWork.Complete())
            {
                var postDto = _mapper.Map<PostDto>(post);

                return Ok(postDto);
            }

            return BadRequest("Failed to add dislike to post");            
        }

        [HttpPost("add-like-to-post/{postId}")]
        public async Task<ActionResult<PostDto>> AddLikeToPost(int postId)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdWithLikes(postId);

            if (post == null) return BadRequest("This post does not exist");

            var postLike = post.Likes
                .Where(x => x.AppUserId == User.GetUserId())
                .SingleOrDefault();

            if (postLike != null) 
            {
                if (postLike.Liked)
                {
                    post.Likes.Remove(postLike);
                    post.LikesCount--;
                }

                if (!postLike.Liked)
                {
                   postLike.Liked = true;
                   post.LikesCount++;
                   post.DislikesCount--;
                }

                _unitOfWork.PostRepository.Update(post);
            }
            else
            {
                var like = new Like
                {
                    Liked = true,
                    AppUserId = User.GetUserId(),
                    PostId = post.Id,
                    // Post = post
                };

                post.LikesCount++;

                post.Likes.Add(like);
            }

            if (await _unitOfWork.Complete())
            {
                var postDto = _mapper.Map<PostDto>(post);

                return Ok(postDto);
            } 

            return BadRequest("Failed to add like to post");
        }

        [HttpPost("add-comment-to-post")]
        public async Task<ActionResult<PostCommentDto>> AddCommentToPost(AddCommentToPostDto addCommentToPostDto)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdWithPostComments(addCommentToPostDto.PostId);

            if (post == null) return BadRequest("This post does not exist");

            if (string.IsNullOrEmpty(addCommentToPostDto.Content)) return BadRequest("Content can not be empty");

            var postComment = new PostComment
            {
                Content = addCommentToPostDto.Content,
                PostId = addCommentToPostDto.PostId,
                Post = post,
                AppUserId = User.GetUserId(),
                AppUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername())
            };

            post.PostComments.Add(postComment);

            var postDto = _mapper.Map<PostCommentDto>(postComment);

            if (await _unitOfWork.Complete()) return Ok(postDto);

            return BadRequest("Failed to add a post comment");
        }

        [HttpGet("get-post-comments/{postId}/{skipPostComments}")]
        public async Task<ActionResult<List<PostCommentDto>>> GetPostComments(int postId, int skipPostComments)
        {
            var postComments = await _unitOfWork.PostRepository.GetPostCommentsByPostId(postId, skipPostComments, Constants.PostCommentParams.TakeCommentPosts);

            return Ok(postComments);
        }

        [HttpGet("get-profile-posts/{username}")]
        public async Task<ActionResult<List<PostDto>>> GetProfilePosts([FromQuery]PaginationParams paginationParams, string username)
        {
            var posts = await _unitOfWork.PostRepository.GetProfilePostsPaginated(paginationParams, username);

            Response.AddPaginationHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPages);

            return Ok(posts);
        }

    }
}
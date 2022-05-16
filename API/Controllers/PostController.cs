using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public PostController(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
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
        public async Task<ActionResult> GetFriendsPost([FromQuery]int skipPosts)
        {
            var posts = await _unitOfWork.PostRepository.GetFriendsPost(User.GetUserId(), skipPosts, Constants.PostsParams.TakePosts);

            return Ok(posts);
        }
    }
}
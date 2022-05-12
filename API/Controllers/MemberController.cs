using API.Constants;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MemberController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public MemberController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet("{username}", Name = "GetMember")]
        public async Task<ActionResult<MemberDto>> GetMember(string username)
        {
            return await _unitOfWork.UserRepository.GetMemberByUsernameAsync(username);           
        }

        [HttpGet("GetMemberByUsername/{username}")]
        public async Task<ActionResult<MemberDto>> GetMemberByUsername(string username)
        {
            var member = await _unitOfWork.UserRepository.GetMemberByUsernameAsync(username);

            if (member == null) return NoContent();

            var currentUser = await _unitOfWork.FriendshipRequestRepository.GetUserWithFriendShipRequests(User.GetUsername());
            var memberUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(member.UserName);

            if (member.UserName.Equals(currentUser.UserName))
            {
                member.FriendshipRequestStatus = FriendRequestState.None;
            }
            else 
            {
                var result = currentUser.FriendshipRequestsSended.Where(x => x.DestinationUserId == memberUser.Id); 
                
                member.FriendshipRequestStatus = result.Count() == 0 ? FriendRequestState.NonRequested : FriendRequestState.Requested;                

                if (result.Count() == 0) 
                {
                    bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), memberUser.Id);

                    if (friendship) 
                    {
                        member.FriendshipRequestStatus = FriendRequestState.Friends;
                    }
                    else
                    {
                        currentUser = await _unitOfWork.FriendshipRequestRepository.GetUserWithFriendShipReceived(currentUser.UserName);

                        result = currentUser.FriendshipRequestsReceived.Where(x => x.SourceUserId == memberUser.Id);

                        member.FriendshipRequestStatus = result.Count() == 0 ? FriendRequestState.NonReceived : FriendRequestState.Received;
                    }
                }
            }

            return member;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(UpdateMemberDto updateMember)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(updateMember, user);

            _unitOfWork.UserRepository.Update(user);

            if(await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0) 
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetMember", new {userName = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) 
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This photo is already a main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to set main photo");            
        }
    }
}
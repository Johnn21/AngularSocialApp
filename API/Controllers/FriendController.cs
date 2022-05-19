using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FriendController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("send-friendship-request/{receiverUsername}")]
        public async Task<ActionResult> SendFriendshipRequest(string receiverUsername) 
        {
            var sourceUser = await _unitOfWork.FriendshipRequestRepository.GetUserWithFriendShipRequests(User.GetUsername());

            if (sourceUser.UserName.Equals(receiverUsername)) return BadRequest("You cannot send yourself a friendship request");

            var destinationUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(receiverUsername);

            if (destinationUser == null) return BadRequest("This user does not exist");

            var friendshipRequest = new FriendshipRequest
            {
                SourceUserId = sourceUser.Id,
                DestinationUserId = destinationUser.Id
            };
            
            if (await _unitOfWork.FriendshipRequestRepository.CheckIfRequestFriendshipAlreadyExists(friendshipRequest)) 
                return BadRequest("You already sended a friendship request to this user");

            bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), destinationUser.Id);

            if (friendship) return BadRequest("You already are a friend with this user");

            sourceUser.FriendshipRequestsSended.Add(friendshipRequest);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to send friendship request");
        }

        [HttpGet("get-friendship-requests/{username}")]
        public async Task<ActionResult<List<FriendRequestsReceiveDto>>> GetFriendshipRequests(string username)
        {
            var currentUser = await _unitOfWork.FriendshipRequestRepository.GetUserWithFriendShipReceived(username); 

            if (currentUser == null) return BadRequest("This user does not exist");

            var requests = new List<FriendRequestsReceiveDto>();

            if (currentUser.FriendshipRequestsReceived.Count() > 0) 
            {
                var requestsReceived = currentUser.FriendshipRequestsReceived;

                foreach(var requestReceived in requestsReceived)
                {
                    var request = new FriendRequestsReceiveDto
                    {
                        SenderUsername = requestReceived.SourceUser.UserName,
                        SenderFirstName = requestReceived.SourceUser.FirstName,
                        SenderLastName = requestReceived.SourceUser.LastName,
                        DateRequestSended = requestReceived.DateRequestSended
                    };

                    requests.Add(request);
                }
            }

            return requests;
        }

        [HttpPost("add-friendship/{username}")]
        public async Task<ActionResult<bool>> AddFriendship(string username)
        {
            var recevingFriendshipUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (recevingFriendshipUser == null) return BadRequest("This user does not exist");

            bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), recevingFriendshipUser.Id);

            if (friendship) return BadRequest("You already have a friendship with this user");

            var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var friendshipCurrentUser = new Friendship
            {
                CurrentUserId = currentUser.Id,
                FriendUserId = recevingFriendshipUser.Id,
            };

            _unitOfWork.FriendshipRepository.AddFriendship(friendshipCurrentUser);

            var friendshipReceivingUser = new Friendship
            {
                CurrentUserId = recevingFriendshipUser.Id,
                FriendUserId = currentUser.Id,
            };

            _unitOfWork.FriendshipRepository.AddFriendship(friendshipReceivingUser);

            await _unitOfWork.FriendshipRequestRepository.DeleteFriendRequest(currentUser.Id, recevingFriendshipUser.Id);

            if(await _unitOfWork.Complete()) return Ok(true);

            return BadRequest("Failed to create friendship");
        }

        [HttpDelete("reject-friendship/{username}")]
        public async Task<ActionResult<bool>> RejectFriendship(string username)
        {
            var recevingFriendshipUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (recevingFriendshipUser == null) return BadRequest("This user does not exist");

            bool friendship = await _unitOfWork.FriendshipRepository.CheckFriendship(User.GetUserId(), recevingFriendshipUser.Id);

            if (friendship) return BadRequest("You already have a friendship with this user");

            await _unitOfWork.FriendshipRequestRepository.DeleteFriendRequest(User.GetUserId(), recevingFriendshipUser.Id);

            if (await _unitOfWork.Complete()) return Ok(true);

            return BadRequest("Failed to reject friendship");
        }

        [HttpGet("get-friends-list")]
        public async Task<ActionResult<List<FriendDto>>> GetFriendsList()
        {
            var friends = await _unitOfWork.FriendshipRepository.GetFriendsByUserId(User.GetUserId());

            return Ok(friends);
        }

        [HttpGet("get-profile-friends-list/{username}")]
        public async Task<ActionResult<PagedList<FriendDto>>> GetProfileFriendsList([FromQuery]PaginationParams paginationParams, string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var friends = await _unitOfWork.FriendshipRepository.GetFriendsByUserIdPaginated(paginationParams, User.GetUserId());

            Response.AddPaginationHeader(friends.CurrentPage, friends.PageSize, friends.TotalCount, friends.TotalPages);

            return Ok(friends);
        }
     }
}
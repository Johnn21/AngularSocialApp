using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public string Description { get; set; }

        public ICollection<FriendshipRequest> FriendshipRequestsReceived { get; set; }
        public ICollection<FriendshipRequest> FriendshipRequestsSended { get; set; }
    }
}
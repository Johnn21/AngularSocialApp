namespace API.DTOs
{
    public class MemberDto
    {  
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public string FriendshipRequestStatus { get; set; }
    }
}
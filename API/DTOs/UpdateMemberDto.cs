namespace API.DTOs
{
    public class UpdateMemberDto
    {
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
    }
}
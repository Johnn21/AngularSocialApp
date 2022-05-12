namespace API.DTOs
{
    public class UserDto
    {
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public int FriendRequestsCount { get; set; }
    }
}
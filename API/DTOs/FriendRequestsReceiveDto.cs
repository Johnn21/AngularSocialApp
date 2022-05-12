namespace API.DTOs
{
    public class FriendRequestsReceiveDto
    {
        public string SenderUsername { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public DateTime DateRequestSended { get; set; }
    }
}
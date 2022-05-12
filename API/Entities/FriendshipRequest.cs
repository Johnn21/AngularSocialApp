namespace API.Entities
{
    public class FriendshipRequest
    {
        public DateTime DateRequestSended { get; set; } = DateTime.Now;

        public AppUser SourceUser { get; set; }
        public string SourceUserId { get; set; }

        public AppUser DestinationUser { get; set; }
        public string DestinationUserId { get; set; }
    }
}
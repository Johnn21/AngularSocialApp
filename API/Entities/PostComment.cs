namespace API.Entities
{
    public class PostComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public int PostId { get; set; }
        public Post Post { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
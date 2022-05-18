namespace API.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public bool Liked { get; set; }

        public string AppUserId { get; set; }
        public int PostId { get; set; }
        // public Post Post { get; set; }
    }
}
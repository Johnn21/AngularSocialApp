namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasPhoto { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public PhotoPost PhotoPost { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
    }
}
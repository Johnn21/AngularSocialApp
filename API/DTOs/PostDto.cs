namespace API.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasPhoto { get; set; }
        public string Username { get; set; }
        public string UserPhoto { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

        public string PhotoUrl { get; set; }
        public ICollection<LikeDto> Likes { get; set; }
    }
}
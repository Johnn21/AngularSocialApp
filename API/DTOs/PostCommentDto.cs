namespace API.DTOs
{
    public class PostCommentDto
    {
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }
        public string UserPhoto { get; set; }
    }
}
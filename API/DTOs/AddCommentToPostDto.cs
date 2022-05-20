namespace API.DTOs
{
    public class AddCommentToPostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}
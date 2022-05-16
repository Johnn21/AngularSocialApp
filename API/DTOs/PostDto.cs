namespace API.DTOs
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasPhoto { get; set; }
        public string Username { get; set; }
        public string UserPhoto { get; set; }

        public string PhotoUrl { get; set; }
    }
}
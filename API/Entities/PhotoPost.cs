using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PhotoPosts")]
    public class PhotoPost
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}
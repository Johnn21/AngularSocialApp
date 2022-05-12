namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateMessageSend { get; set; } = DateTime.Now;

        public string SenderUserId { get; set; }
        public AppUser SenderUser { get; set; }

        public string ReceiverUserId { get; set; }
        public AppUser ReceiverUser { get; set; }
    }
}
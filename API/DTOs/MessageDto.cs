namespace API.DTOs
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime DateMessageSend { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
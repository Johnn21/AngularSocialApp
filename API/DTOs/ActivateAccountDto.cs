namespace API.DTOs
{
    public class ActivateAccountDto
    {
        public string Username { get; set; }
        public string AccountConfirmationToken { get; set; }
    }
}
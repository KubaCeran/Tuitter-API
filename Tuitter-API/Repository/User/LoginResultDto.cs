namespace Tuitter_API.Repository.User
{
    public class LoginResultDto
    {
        public bool IsError { get; set; }
        public string ResponseMsg { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; }
    }
}

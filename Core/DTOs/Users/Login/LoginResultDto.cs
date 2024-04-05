namespace Core.DTOs.Users.Login
{
    public class LoginResultDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}

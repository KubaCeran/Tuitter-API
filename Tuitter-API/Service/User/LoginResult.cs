namespace Tuitter_API.Service.User
{
    public class AccountResult
    {
        public string Username { get; set; }
    }

    public class LoginResult : AccountResult
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

namespace Core.Options.Auth
{
    public class AuthConfiguration
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? Secret { get; set; }
    }
}

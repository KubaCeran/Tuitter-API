namespace Tuitter_API.Service
{
    public class AuthConfiguration
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Secret { get; set; }
    }
}

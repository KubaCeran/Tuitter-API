using Microsoft.AspNetCore.Identity;

namespace Tuitter_API.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}

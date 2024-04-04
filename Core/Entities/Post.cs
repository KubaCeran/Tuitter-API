using Core.Entities.Base;

namespace Core.Entities
{
    public class Post : EntityBase
    {
        public string Body { get; set; } = null!;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public int UserId { get; set; }
        public Post? ParentPost { get; set; }
        public int? ParentPostId { get; set; }
        public ICollection<Post> Replies { get; set; } = new List<Post>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}

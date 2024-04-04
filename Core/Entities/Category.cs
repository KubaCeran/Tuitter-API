using Core.Entities.Base;

namespace Core.Entities
{
    public class Category : EntityBase
    {
        public string Title { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}

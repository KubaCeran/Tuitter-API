namespace Core.DTOs.Posts
{
    public class PostDto
    {
        public int UserId { get; set; }
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> CategoriesNames { get; set; } = new List<string>();
    }
}

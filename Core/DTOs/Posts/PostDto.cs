namespace Core.DTOs.Posts
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int NextLevelRepliesCount { get; set; }
        public IEnumerable<string> CategoriesNames { get; set; } = new List<string>();
    }
}

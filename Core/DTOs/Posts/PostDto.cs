namespace Core.DTOs.Posts
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Headline { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}

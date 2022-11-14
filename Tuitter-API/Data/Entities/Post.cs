namespace Tuitter_API.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Body { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public User User { get; set; }
        public int UserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}

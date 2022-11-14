namespace Tuitter_API.Repository.Post
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string CretaedByUsername { get; set; }
        public string Headline { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
    }
}

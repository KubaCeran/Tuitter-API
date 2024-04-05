using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required]
        public string Body { get; set; } = null!;
        public string[]? Categories { get; set; }
        public int? ParentPostId { get; set; }
    }
}

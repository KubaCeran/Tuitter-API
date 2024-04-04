using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required]
        public string Body { get; set; } = null!;
        [Required]
        public string[] Categories { get; set; } = null!;
    }
}

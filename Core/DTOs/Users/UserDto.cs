using Core.DTOs.Posts;

namespace Core.DTOs.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public IEnumerable<PostDto>? Posts { get; set; }
    }
}

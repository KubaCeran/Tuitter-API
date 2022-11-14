using Tuitter_API.Repository.Post;

namespace Tuitter_API.Repository.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}

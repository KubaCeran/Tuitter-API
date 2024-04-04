using Core.DTOs.Posts;

namespace Infrastructure.Services.Posts
{
    public interface IPostService
    {
        Task AddPost(CreatePostDto postDto, int userId);
        Task DeletePost(int postId, int userId);
        IEnumerable<PostDto> GetAllPostsForCategory(string categoryName);
        IEnumerable<PostDto> GetAllPosts();
        IEnumerable<PostDto> GetAllPostsForUser(int userId);
    }
}

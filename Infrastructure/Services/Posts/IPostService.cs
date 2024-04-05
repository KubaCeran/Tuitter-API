using Core.DTOs.Posts;

namespace Infrastructure.Services.Posts
{
    public interface IPostService
    {
        Task AddPost(CreatePostDto postDto, int userId, CancellationToken cancellationToken);
        Task DeletePost(int postId, int userId, CancellationToken cancellationToken);
        IEnumerable<PostDto> GetAllPostsForCategory(string categoryName);
        IEnumerable<PostDto> GetAllPostsByParentId(int? ParentPostId);
        IEnumerable<PostDto> GetAllPostsForUser(int userId);
    }
}

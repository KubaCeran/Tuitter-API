using Core.DTOs.Posts;
using Core.Options.Pagination;

namespace Infrastructure.Services.Posts
{
    public interface IPostService
    {
        Task AddPost(CreatePostDto postDto, int userId, CancellationToken cancellationToken);
        Task DeletePost(int postId, int userId, CancellationToken cancellationToken);
        PagedList<PostDto> GetAllPostsForCategory(PaginationOptions paginationOptions, string categoryName);
        PagedList<PostDto> GetAllPostsByParentId(PaginationOptions paginationOptions, int? ParentPostId);
        PagedList<PostDto> GetAllPostsForUser(PaginationOptions paginationOptions, int userId);
    }
}

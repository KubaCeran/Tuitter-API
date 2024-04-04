using Core.Entities;

namespace Infrastructure.Repositories.Posts
{
    public interface IPostRepository
    {
        Task AddPost(Post post);
        Task DeletePost(Post post);
        IQueryable<Post> GetAllPosts();
        IQueryable<Post> GetAllPostsByUserId(int userId);
        IQueryable<Post> GetAllPostsByCategoryName(string categoryName);
        Task<Post> GetSinglePost(int id);
    }
}

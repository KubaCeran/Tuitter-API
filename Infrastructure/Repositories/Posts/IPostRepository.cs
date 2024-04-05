using Core.Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Posts
{
    public interface IPostRepository : IBaseCrudAsyncRepository<Post>, IBaseCrudRepository<Post>
    {
        IQueryable<Post> GetAllPostsWithCategories();
        IQueryable<Post> GetAllPostsByUserId(int userId);
        IQueryable<Post> GetAllPostsByCategoryName(string categoryName);
    }
}

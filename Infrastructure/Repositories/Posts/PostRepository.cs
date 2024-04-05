using Core.Entities;
using Infrastructure.DataContext;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Posts
{
    public class PostRepository(TuitterContext context) : BaseRepository<Post>(context), IPostRepository
    {

        public IQueryable<Post> GetAllPostsWithCategoriesAndReplies()
        {
            return context.Posts.Include(x => x.Categories).Include(x => x.Replies);
        }

        public IQueryable<Post> GetAllPostsByCategoryName(string categoryName)
        {
            return context.Posts
                .Include(x => x.Categories)
                .Where(x => x.Categories.Any(y => y.Title.ToLower() == categoryName.ToLower()));
        }

        public IQueryable<Post> GetAllPostsByUserId(int userId)
        {
            return context.Posts
                .Include(x => x.Categories)
                .Where(x => x.UserId == userId);
        }
    }
}

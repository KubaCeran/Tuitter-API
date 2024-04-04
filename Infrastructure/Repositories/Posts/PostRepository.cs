using Core.Entities;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Posts
{

    public class PostRepository(TuitterContext context) : IPostRepository
    {
        public async Task AddPost(Post post)
        {
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
        }

        public async Task DeletePost(Post post)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }

        public IQueryable<Post> GetAllPosts()
        {
            return context.Posts.Include(x => x.Categories);
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

        public async Task<Post> GetSinglePost(int postId)
        {
            return await context.Posts.FirstOrDefaultAsync(x => x.Id == postId) ?? throw new Exception($"Post with given Id: {postId} doesn't exist");
        }
    }
}

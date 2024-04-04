using Core.DTOs.Posts;
using Core.Entities;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Posts
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPosts();
        void AddPost(Post post);
        Task<bool> SaveAllAsync();
        void DeletePost(Post post);

        Task<List<PostDto>> GetAllPostsForUser(int id);
        Task<List<PostDto>> GetAllPostsForCategory(int id);
        Task<Post> GetSinglePost(int id);


    }
    public class PostRepository : IPostRepository
    {
        private readonly TuitterContext _Context;

        public PostRepository(TuitterContext context)
        {
            _Context = context;
        }

        public async void AddPost(Post post)
        {
             await _Context.Posts.AddAsync(post);
        }

        public void DeletePost(Post post)
        {
             _Context.Posts.Remove(post);
        }

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _Context.Posts.Select(x => new PostDto
            {
                PostId = x.Id,
                Headline = x.Headline,
                UserId = x.UserId,
                Body = x.Body,
                CreatedAt = x.CreationTime,
                CategoryName = x.Category.Title,
                CategoryId = x.CategoryId
            }).ToListAsync();

            return posts;
        }

        public async Task<List<PostDto>> GetAllPostsForCategory(int id)
        {
            var posts = await _Context.Posts.Where(x => x.CategoryId == id)
                .Select(x => new PostDto
                {
                    UserId = x.UserId,
                    Body = x.Body,
                    CreatedAt = x.CreationTime,
                    Headline = x.Headline,
                    PostId = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Title
                }).ToListAsync();

            return posts;
        }

        public async Task<List<PostDto>> GetAllPostsForUser(int id)
        {
            var posts = await _Context.Posts.Where(x => x.UserId == id)
                .Select(x => new PostDto
                {
                    UserId = x.UserId,
                    Body = x.Body,
                    CreatedAt = x.CreationTime,
                    Headline = x.Headline,
                    PostId = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Title
                }).ToListAsync();

            return posts;
        }

        public async Task<Post> GetSinglePost(int id)
        {
            return await _Context.Posts.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync() > 0;
        }
    }
}

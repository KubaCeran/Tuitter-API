using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext;

namespace Tuitter_API.Repository.Post
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPosts();
        void AddPost(Data.Entities.Post post);
        Task<bool> SaveAllAsync();
        void DeletePost(Data.Entities.Post post);

        Task<List<PostDto>> GetAllPostsForUser(int id);
        Task<List<PostDto>> GetAllPostsForCategory(int id);
        Task<Data.Entities.Post> GetSinglePost(int id);


    }
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;

        public PostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async void AddPost(Data.Entities.Post post)
        {
             await _dataContext.Posts.AddAsync(post);
        }

        public void DeletePost(Data.Entities.Post post)
        {
             _dataContext.Posts.Remove(post);
            
        }

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _dataContext.Posts.Select(x => new PostDto
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
            var posts = await _dataContext.Posts.Where(x => x.CategoryId == id)
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
            var posts = await _dataContext.Posts.Where(x => x.UserId == id)
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

        public async Task<Data.Entities.Post> GetSinglePost(int id)
        {
            return await _dataContext.Posts.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

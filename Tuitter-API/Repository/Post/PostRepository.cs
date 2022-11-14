using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext;

namespace Tuitter_API.Repository.Post
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPosts();
        void AddPost(Data.Entities.Post post);
        Task<bool> SaveAllAsync();

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

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _dataContext.Posts.Select(x => new PostDto
            {
                PostId = x.Id,
                Headline = x.Headline,
                CretaedByUsername = x.User.UserName,
                Body = x.Body,
                CreatedAt = x.CreationTime,
                CategoryName = x.Category.Title
            }).ToListAsync();

            return posts;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

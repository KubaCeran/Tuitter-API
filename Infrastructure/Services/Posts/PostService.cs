using AutoMapper;
using Core.DTOs.Posts;
using Core.Entities;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Posts;

namespace Infrastructure.Services.Posts
{
    public class PostService(
        IPostRepository postRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper) : IPostService
    {
        public async Task AddPost(CreatePostDto postDto, int userId)
        {
            var categories = categoryRepository.FindCategoryByName(postDto.Categories);
            var postEntity = mapper.Map<Post>(postDto);

            postEntity.UserId = userId;
            postEntity.Categories = categories.ToList();

            await postRepository.AddPost(postEntity);
        }

        public async Task DeletePost(int postId, int userId)
        {
            var post = await postRepository.GetSinglePost(postId);
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var postsQuery = postRepository.GetAllPosts().OrderBy(x => x.CreationTime);
            return mapper.ProjectTo<PostDto>(postsQuery);
        }

        public IEnumerable<PostDto> GetAllPostsForCategory(string categoryName)
        {
            var postsQuery = postRepository.GetAllPostsByCategoryName(categoryName).OrderBy(x => x.CreationTime);
            return mapper.ProjectTo<PostDto>(postsQuery);
        }

        public IEnumerable<PostDto> GetAllPostsForUser(int userId)
        {
            var postsQuery = postRepository.GetAllPostsByUserId(userId).OrderBy(x => x.CreationTime);
            return mapper.ProjectTo<PostDto>(postsQuery);
        }
    }
}

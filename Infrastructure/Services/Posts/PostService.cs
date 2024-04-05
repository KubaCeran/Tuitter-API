using AutoMapper;
using Core.DTOs.Posts;
using Core.Entities;
using Infrastructure.Middlewares.Exceptions;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Posts;

namespace Infrastructure.Services.Posts
{
    public class PostService(
        IPostRepository postRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper) : IPostService
    {
        public async Task AddPost(CreatePostDto postDto, int userId, CancellationToken cancellationToken)
        {
            var categories = categoryRepository.FindCategoryByName(postDto.Categories);
            var postEntity = mapper.Map<Post>(postDto);

            postEntity.UserId = userId;
            postEntity.Categories = categories.ToList();

            await postRepository.AddAsync(postEntity, cancellationToken);
        }

        public async Task DeletePost(int postId, int userId, CancellationToken cancellationToken)
        {
            var post = await postRepository.GetByIdAsync(postId, false, cancellationToken);
            if (post.UserId == userId)
                await postRepository.DeleteByIdAsync(postId, cancellationToken);
            else
                throw new ForbiddenException("Cannot delete someones else post");
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var postsQuery = postRepository.GetAllPostsWithCategories().OrderBy(x => x.CreationTime);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tuitter_API.Data.Entities;
using Tuitter_API.Repository.Categories;
using Tuitter_API.Repository.Post;
using Tuitter_API.Service.User;

namespace Tuitter_API.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostsController(IPostRepository postRepository, ICategoryRepository categoryRepository, ILoggedUserService loggedUserService)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _loggedUserService = loggedUserService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<PostDto>>> GetAllPosts()
        {

            return await _postRepository.GetAllPosts();
        }

        [HttpPost("add")]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postInput)
        {
            var userId = await _loggedUserService.GetLoggedUserId(User);
            var category = await _categoryRepository.FindCategoryByName(postInput.CategoryName);

            var post = new Post();

            if (category == null)
            {
                post.Body = postInput.Body;
                post.Headline = postInput.Headline;
                post.Category = new Category { Title = postInput.CategoryName };
                post.UserId = userId;
            }
            else
            {
                post.Body = postInput.Body;
                post.Headline = postInput.Headline;
                post.Category = category;
                post.UserId = userId;
            }
            _postRepository.AddPost(post);

            if (await _postRepository.SaveAllAsync())
            {
                return Ok(new PostDto
                {
                    PostId = post.Id,
                    Body = post.Body,
                    CategoryName = post.Category.Title,
                    CreatedAt = post.CreationTime,
                    CretaedByUsername = post.User.UserName,
                    Headline = post.Headline
                });
            }
            else
            {
                return BadRequest("Failed to add post");
            }
        }
    }
}

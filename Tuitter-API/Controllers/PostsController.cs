using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tuitter_API.Data.Entities;
using Tuitter_API.Repository.Categories;
using Tuitter_API.Repository.Post;
using Tuitter_API.Service;
using Tuitter_API.Service.User;

namespace Tuitter_API.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly IUserService _userService;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostsController(IPostRepository postRepository, ICategoryRepository categoryRepository, ILoggedUserService loggedUserService, IUserService userService)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _loggedUserService = loggedUserService;
            _userService = userService;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostDto>>> GetAllPosts()
        {

            var posts = await  _postRepository.GetAllPosts();
            if (posts.Count() == 0)
                return BadRequest("Nobody created post yet");
            return Ok(posts);
        }

        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostDto>>> GetAllPostsForUser(int id)
        {
            var posts = await _postRepository.GetAllPostsForUser(id);
            if (posts.Count() == 0)
                return BadRequest("No posts found");
            return Ok(posts);
        }

        [HttpGet("category/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostDto>>> GetAllPostsForCategory(int id)
        {
            var posts = await _postRepository.GetAllPostsForCategory(id);
            if (posts.Count() == 0)
                return BadRequest("No posts found");
            return Ok(posts);
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
                    UserId = post.UserId,
                    Headline = post.Headline
                });
            }
            else
            {
                return BadRequest("Failed to add post");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var userId = await _loggedUserService.GetLoggedUserId(User);

            var post = await _postRepository.GetSinglePost(id);

            if(post == null)
            {
                return BadRequest("Cannot find the post");

            }

            if (post.UserId == userId)
            {
                _postRepository.DeletePost(post);
            }
            else
            {
                return BadRequest("Cannot delete someones else post");
            }

            if (await _postRepository.SaveAllAsync()) return Ok("Post deleted successfully");

            return BadRequest("Problem deleting the post");
        }
    }
}

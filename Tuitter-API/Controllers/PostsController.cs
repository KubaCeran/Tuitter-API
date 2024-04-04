using Core.DTOs.Posts;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Posts;
using Infrastructure.Services.Posts;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tuitter_API.Controllers
{
    public class PostsController(
        IPostRepository postRepository,
        ICategoryRepository categoryRepository,
        ILoggedUserService loggedUserService,
        IUserService userService,
        IPostService postService) : BaseApiController
    {
        [HttpGet("all")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDto>> GetAllPosts()
        {
            var posts =  postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDto>> GetAllPostsForUser(int userId)
        {
            var posts = postService.GetAllPostsForUser(userId);
            return Ok(posts);
        }

        [HttpGet("category/{categoryName}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDto>> GetAllPostsForCategory(string categoryName)
        {
            var posts = postService.GetAllPostsForCategory(categoryName);
            return Ok(posts);
        }

        [HttpPost("add")]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postInput)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);
            await postService.AddPost(postInput, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);

            var post = await postRepository.GetSinglePost(id);

            if(post == null)
            {
                return BadRequest("Cannot find the post");

            }

            if (post.UserId == userId)
            {
                postRepository.DeletePost(post);
            }
            else
            {
                return BadRequest("Cannot delete someones else post");
            }

            return Ok("Post deleted successfully");

           //return BadRequest("Problem deleting the post");
        }
    }
}

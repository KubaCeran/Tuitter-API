using Core.DTOs.Posts;
using Infrastructure.Services.Posts;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tuitter_API.Controllers
{
    public class PostsController(
        ILoggedUserService loggedUserService,
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
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postInput, CancellationToken cancellationToken)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);
            await postService.AddPost(postInput, userId, cancellationToken);
            return Ok();
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId, CancellationToken cancellationToken)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);
            await postService.DeletePost(postId, userId, cancellationToken);
            return Ok();
        }
    }
}

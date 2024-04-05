using Core.DTOs.Posts;
using Core.Options.Pagination;
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
        public ActionResult<PagedList<PostDto>> GetAllPostsAtGivenLevel(
            [FromQuery] PaginationOptions paginationOptions,
            int? parentPostId)
        {
            var posts =  postService.GetAllPostsByParentId(paginationOptions, parentPostId);
            return Ok(posts);
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public ActionResult<PagedList<PostDto>> GetAllPostsForUser(
            [FromQuery] PaginationOptions paginationOptions,
            int userId)
        {
            var posts = postService.GetAllPostsForUser(paginationOptions, userId);
            return Ok(posts);
        }

        [HttpGet("category/{categoryName}")]
        [AllowAnonymous]
        public ActionResult<PagedList<PostDto>> GetAllPostsForCategory(
            [FromQuery] PaginationOptions paginationOptions,
            string categoryName)
        {
            var posts = postService.GetAllPostsForCategory(paginationOptions, categoryName);
            return Ok(posts);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto postInput, CancellationToken cancellationToken)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);
            await postService.AddPost(postInput, userId, cancellationToken);
            return Ok();
        }

        [HttpDelete("{postId}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int postId, CancellationToken cancellationToken)
        {
            var userId = await loggedUserService.GetLoggedUserId(User);
            await postService.DeletePost(postId, userId, cancellationToken);
            return Ok();
        }
    }
}

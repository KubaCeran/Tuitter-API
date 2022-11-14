using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tuitter_API.Repository.Post;
using Tuitter_API.Repository.User;
using Tuitter_API.Service;

namespace Tuitter_API.Controllers
{
    [AllowAnonymous]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IPostRepository _postRepository;

        public UsersController(IUserService userService, IPostRepository postRepository)
        {
            _userService = userService;
            _postRepository = postRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResultDto>> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _userService.RegisterUser(registerDto);
            if(response.IsError)
                return BadRequest(response.ResponseMsg);
            return Ok(response.ResponseMsg);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] RegisterDto registerDto)
        {
        
            var response = await _userService.LoginUser(registerDto);
            if (response.IsError)
                return BadRequest(response.ResponseMsg);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            var usersPosts = await _postRepository.GetAllPostsForUser(id);

            var userDto = new UserDto { UserId = user.Id, Username = user.UserName, Posts = usersPosts };
            
            return Ok(userDto);
        }
    }
}

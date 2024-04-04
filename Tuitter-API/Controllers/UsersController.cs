using Core.DTOs.Users;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Infrastructure.Repositories.Posts;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (response.IsError)
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
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            //var usersPosts = await _postRepository.GetAllPostsForUser(id);

            var userDto = new UserDto { UserId = user.Id, Username = user.UserName, /*Posts = usersPosts */};

            return Ok(userDto);
        }
    }
}

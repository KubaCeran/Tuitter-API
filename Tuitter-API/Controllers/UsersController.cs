using Core.DTOs.Users;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tuitter_API.Controllers
{
    [AllowAnonymous]
    public class UsersController(IUserService userService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await userService.RegisterUser(registerDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] RegisterDto registerDto)
        {

            var response = await userService.LoginUser(registerDto);
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(int userId, CancellationToken cancellationToken)
        {
            var response = await userService.GetUserById(userId, cancellationToken);
            return Ok(response);
        }
    }
}

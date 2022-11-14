using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tuitter_API.Repository.User;
using Tuitter_API.Service;

namespace Tuitter_API.Controllers
{
    [AllowAnonymous]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
    }
}

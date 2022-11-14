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
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> Register([FromBody] RegisterDto registerDto)
        {
            return await MethodWrapper(async () =>
            {
                return await _userService.RegisterUser(registerDto);
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult>> Login([FromBody] RegisterDto registerDto)
        {
            return await MethodWrapper(async () =>
            {
                return await _userService.LoginUser(registerDto);
            });
        }
    }
}

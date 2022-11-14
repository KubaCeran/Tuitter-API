using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tuitter_API.Service;

namespace Tuitter_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult<ServiceResult<T>>> MethodWrapper<T>(Func<Task<ServiceResult<T>>> methodInternal)
        {
            var result = await methodInternal();

            if (result.IsError)
            {
                return BadRequest(result.ErrorsMessage);
            }

            return Ok(result);
        }

        protected async Task<ActionResult<ServiceResult>> MethodWrapper(Func<Task<ServiceResult>> methodInternal)
        {
            var result = await methodInternal();

            if (result.IsError)
            {
                return BadRequest(result.ErrorsMessage);
            }

            return Ok(result);
        }
    }
}

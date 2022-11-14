using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Tuitter_API.Service.User
{
    public interface ILoggedUserService
    {
        Task<int> GetLoggedUserId(ClaimsPrincipal claimsPrincipal);
    }

    public class LoggedUserService : ILoggedUserService
    {
        private readonly UserManager<Data.Entities.User> _userManager;

        public LoggedUserService(UserManager<Data.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<int> GetLoggedUserId(ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            var account = await _userManager.FindByNameAsync(username);

            return account.Id;
        }
    }
}

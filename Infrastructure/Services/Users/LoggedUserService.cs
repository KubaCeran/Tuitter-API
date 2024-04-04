using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services.Users
{
    public interface ILoggedUserService
    {
        Task<int> GetLoggedUserId(ClaimsPrincipal claimsPrincipal);
    }

    public class LoggedUserService : ILoggedUserService
    {
        private readonly UserManager<Core.Entities.User> _userManager;

        public LoggedUserService(UserManager<Core.Entities.User> userManager)
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

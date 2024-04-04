using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services.Users
{

    public class LoggedUserService(UserManager<User> userManager) : ILoggedUserService
    {
        public async Task<int> GetLoggedUserId(ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Couldn't find claim type of type 'Name'");
            var account = await userManager.FindByNameAsync(username) ?? throw new Exception("Couldn't find user");
            return account.Id;
        }
    }
}

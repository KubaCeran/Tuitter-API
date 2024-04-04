using System.Security.Claims;

namespace Infrastructure.Services.Users
{
    public interface ILoggedUserService
    {
        Task<int> GetLoggedUserId(ClaimsPrincipal claimsPrincipal);
    }
}

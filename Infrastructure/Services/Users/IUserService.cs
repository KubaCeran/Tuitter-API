using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Core.Entities;

namespace Infrastructure.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<LoginResultDto> LoginUser(RegisterDto registerDto);
        Task<RegisterResultDto> RegisterUser(RegisterDto registerDto);
    }
}
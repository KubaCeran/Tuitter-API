using Core.DTOs.Users;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;

namespace Infrastructure.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken);
        Task<LoginResultDto> LoginUser(LoginDto registerDto);
        Task RegisterUser(RegisterDto registerDto);
    }
}
using Core.DTOs.Options.Auth;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Infrastructure.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Users;

public interface IUserService
{
    Task<RegisterResultDto> RegisterUser(RegisterDto registerDto);
    Task<LoginResultDto> LoginUser(RegisterDto registerDto);
    Task<Core.Entities.User> GetUserById(int id);

}
public class UserService : IUserService
{
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IOptions<AuthConfiguration> _authOptions;
    private readonly TuitterContext _dataContext;

    public UserService(
        UserManager<Core.Entities.User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<AuthConfiguration> authOptions,
        TuitterContext dataContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authOptions = authOptions;
        _dataContext = dataContext;
    }

    public async Task<Core.Entities.User> GetUserById(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<LoginResultDto> LoginUser(RegisterDto registerDto)
    {
        var user = await _userManager.FindByNameAsync(registerDto.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, registerDto.Password))
        {
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            var token = GetToken(authClaims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new LoginResultDto
            {
                Token = tokenString,
                Username = user.UserName,
                ExpiresAt = token.ValidTo,
                IsError = false,
                ResponseMsg = "User logged in successfully!"
            };
        }
        return new LoginResultDto
        {
            IsError = true,
            ResponseMsg = "Incorrect username or password!"
        };
    }

    public async Task<RegisterResultDto> RegisterUser(RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerDto.Username);
        if (userExists != null)
        {
            return new RegisterResultDto
            {
                IsError = true,
                ResponseMsg = "Username taken!"
            };
        }

        var user = new Core.Entities.User()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        var userRole = registerDto.UserRole.ToString();
        await _roleManager.CreateAsync(new IdentityRole<int> { Name =  userRole});
        await _userManager.AddToRoleAsync(user, userRole);
        var errorList = result.Errors.Select(x => x.Description).ToList();
        if (result.Succeeded)
        {
            return new RegisterResultDto
            {
                IsError = false,
                ResponseMsg = "User created!"
            };
        }
        else
        {
            return new RegisterResultDto
            {
                IsError = true,
                ResponseMsg = String.Join(' ', errorList)
            };
        }
    }
    private JwtSecurityToken GetToken(IList<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Value.Secret));

        return new JwtSecurityToken(
            issuer: _authOptions.Value.ValidIssuer,
            audience: _authOptions.Value.ValidAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}


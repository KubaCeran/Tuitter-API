using Core.DTOs.Options.Auth;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Core.Entities;
using Infrastructure.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Users;

public class UserService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<int>> roleManager,
    IOptions<AuthConfiguration> authOptions,
    TuitterContext dataContext) : IUserService
{
    public async Task<User> GetUserById(int id)
    {
        return await dataContext.Users.FindAsync(id);
    }

    public async Task<LoginResultDto> LoginUser(RegisterDto registerDto)
    {
        var user = await userManager.FindByNameAsync(registerDto.Username);
        if (user != null && await userManager.CheckPasswordAsync(user, registerDto.Password))
        {
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            var token = GetToken(authClaims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            user.LastLoginDate = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

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
        var userExists = await userManager.FindByNameAsync(registerDto.Username);
        if (userExists != null)
        {
            return new RegisterResultDto
            {
                IsError = true,
                ResponseMsg = "Username taken!"
            };
        }

        var user = new User()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);
        var userRole = registerDto.UserRole.ToString();
        await roleManager.CreateAsync(new IdentityRole<int> { Name =  userRole});
        await userManager.AddToRoleAsync(user, userRole);
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
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Value.Secret));

        return new JwtSecurityToken(
            issuer: authOptions.Value.ValidIssuer,
            audience: authOptions.Value.ValidAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}


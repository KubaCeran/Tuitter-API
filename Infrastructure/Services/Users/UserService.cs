using AutoMapper;
using Core.DTOs.Posts;
using Core.DTOs.Users;
using Core.DTOs.Users.Login;
using Core.DTOs.Users.Register;
using Core.Entities;
using Core.Options.Auth;
using Infrastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    IMapper mapper) : IUserService
{
    public async Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Include(x => x.Posts).ThenInclude(x => x.Categories).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken) ??
            throw new BadRequestException($"User with ID:{userId} not found");

        return mapper.Map<UserDto>(user, opt => opt.AfterMap((src, dest) =>
        {
            dest.Posts = mapper.Map<IEnumerable<PostDto>>(user.Posts);
        }));
    }

    public async Task<LoginResultDto> LoginUser(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is not null && await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            var token = GetToken(authClaims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            user.LastLoginDate = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return new LoginResultDto
            {
                Token = tokenString,
                ExpiresAt = token.ValidTo,
            };
        }
        throw new BadRequestException("Incorrect login credentials!");
    }

    public async Task RegisterUser(RegisterDto registerDto)
    {
        var userExists = await userManager.FindByNameAsync(registerDto.Username);
        if (userExists is not null)
            throw new ConflictException("Username taken!");

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
        if (!result.Succeeded)
            throw new Exception("Something went wrong during creation of a user");
    }
    private JwtSecurityToken GetToken(IList<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Value.Secret!));

        return new JwtSecurityToken(
            issuer: authOptions.Value.ValidIssuer,
            audience: authOptions.Value.ValidAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}


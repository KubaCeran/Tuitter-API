using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tuitter_API.Data.DataContext;
using Tuitter_API.Repository.User;

namespace Tuitter_API.Service;

    public interface IUserService
    {
        Task<RegisterResultDto> RegisterUser(RegisterDto registerDto);
        Task<LoginResultDto> LoginUser(RegisterDto registerDto);
        Task<Data.Entities.User> GetUserById(int id);

}
public class UserService : IUserService
    {
        private readonly UserManager<Data.Entities.User> _userManager;
        private readonly IOptions<AuthConfiguration> _authOptions;
        private readonly DataContext _dataContext;

    public UserService(UserManager<Data.Entities.User> userManager,
        IOptions<AuthConfiguration> authOptions,
        DataContext dataContext)
    {
        _userManager = userManager;
        _authOptions = authOptions;
        _dataContext = dataContext;
    }

    public async Task<Data.Entities.User> GetUserById(int id)
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
            if(userExists != null)
            {
                return new RegisterResultDto 
                { 
                    IsError = true, 
                    ResponseMsg = "Username taken!" 
                };
            }

            var user = new Data.Entities.User()
            {
                UserName = registerDto.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
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

        var token = new JwtSecurityToken(
            issuer: _authOptions.Value.ValidIssuer,
            audience: _authOptions.Value.ValidAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}


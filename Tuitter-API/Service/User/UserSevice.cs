using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tuitter_API.Repository.User;
using Tuitter_API.Service.User;

namespace Tuitter_API.Service;

    public interface IUserService
    {
        Task<ServiceResult> RegisterUser(RegisterDto registerDto);
        Task<ServiceResult> LoginUser(RegisterDto registerDto);
    }
    public class UserSevice : IUserService
    {
        private readonly UserManager<Data.Entities.User> _userManager;
        private readonly IOptions<AuthConfiguration> _authOptions;

        public UserSevice(UserManager<Data.Entities.User> userManager,
        IOptions<AuthConfiguration> authOptions)
        {
            _userManager = userManager;
            _authOptions = authOptions;
        }

        public async Task<ServiceResult> LoginUser(RegisterDto registerDto)
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

                return ServiceResult<LoginResult>.WithSuccess(new LoginResult
                {
                    Token = tokenString,
                    Username = user.UserName,
                    ExpiresAt = token.ValidTo
                });
            }

            return ServiceResult.WithErrors("Username or password incorrect");
        }

        public async Task<ServiceResult> RegisterUser(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if(userExists != null)
            {
                return ServiceResult.WithErrors("Username taken");
            }

            var user = new Data.Entities.User()
            {
                UserName = registerDto.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(result.Succeeded)
            {
                return ServiceResult.WithSuccess();
            }
            else
            {
                var errors = result.Errors.Select(x => x.Description).ToArray();
                return ServiceResult.WithErrors(errors);
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


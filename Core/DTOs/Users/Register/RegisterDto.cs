using Core.DTOs.Users.Login;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Users.Register
{
    public class RegisterDto : LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;
        public UserRoles UserRole { get; set; }
    }
}

using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Users.Register
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
        public UserRoles UserRole { get; set; }
    }
}

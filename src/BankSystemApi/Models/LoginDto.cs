using System.ComponentModel.DataAnnotations;

namespace BankSystemApi.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalied email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
    }
}

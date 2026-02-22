using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankSystemApi.Entities
{
    [Index(nameof(Email),IsUnique =true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage ="Invalied email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        public Client? Client { get; set; }

        
    }
}

using System.ComponentModel.DataAnnotations;

namespace BankSystemApi.Models
{
    public class ClientCreateDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Field must be exactly 11 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
    }
}

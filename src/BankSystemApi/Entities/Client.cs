using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystemApi.Entities
{
    [Index(nameof(Email),IsUnique =true)]
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50,MinimumLength =2,ErrorMessage ="First name must be between 2 and 50")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50,MinimumLength =2,ErrorMessage ="Last name must be between 2 and 50")]
        public string LastName { get; set; }= string.Empty;

        [Phone(ErrorMessage ="Invalid phone number")]
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Field must be exactly 11 characters")]
        public string Phone { get; set; }= string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        public User? User { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        // ═════════════════════════════════════════════════════════
        // NAVIGATION PROPERTY (NOT stored in database!)
        // ═════════════════════════════════════════════════════════


        // This is a LIST of accounts
        // Think: "Show me all accounts this person owns"
        // NOT stored in Clients table!
        // Filled by EF Core when you ask for it (.Include)
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}

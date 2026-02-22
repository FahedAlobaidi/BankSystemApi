using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.Entities
{
    [Index(nameof(AccountNumber), IsUnique = true)]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "account number must contains only digits")]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Account balance cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }

        
        public string Currency { get; set; } = "JOD";

        public string Status { get; set; } = "Active";

        // ← FOREIGN KEY (Points to Client table)
        // Example: ClientId = aaaa-bbbb-cccc-dddd
        // This says: "I belong to Client with Id = aaaa-bbbb-cccc-dddd"
        public Guid ClientId { get; set; }

        // ═════════════════════════════════════════════════════════
        // NAVIGATION PROPERTY (NOT stored in database!)
        // ═════════════════════════════════════════════════════════

        // This is a REFERENCE to the owner (Client)
        // Think: "Show me who owns this account"
        // NOT stored in Accounts table!
        // Filled by EF Core when you ask for it (.Include)
        public Client? Client { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}

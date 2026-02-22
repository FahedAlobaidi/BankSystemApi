using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystemApi.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RunningBalance { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public TransactionType TransactionType { get; set; } 

        
        public string? Description { get; set; }

        
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        TransferIn,
        TransferOut
    }
}

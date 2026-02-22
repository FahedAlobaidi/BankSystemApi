using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BankSystemApi.Entities;

namespace BankSystemApi.Models
{
    public class TransactionDto
    {
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RunningBalance { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        public string? Description { get; set; }

        public Guid AccountId { get; set; }
    }
}

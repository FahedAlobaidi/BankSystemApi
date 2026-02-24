using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankSystemApi.Models
{
    public class AccountCreateDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "Account balance cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }

        
        public string Currency { get; set; }
    }
}

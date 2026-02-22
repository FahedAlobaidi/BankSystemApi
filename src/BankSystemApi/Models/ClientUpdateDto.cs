using System.ComponentModel.DataAnnotations;

namespace BankSystemApi.Models
{
    public class ClientUpdateDto
    {
        
        public string FirstName { get; set; } = string.Empty;

        
        public string LastName { get; set; } = string.Empty;

        
        public string Phone { get; set; } = string.Empty;

        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

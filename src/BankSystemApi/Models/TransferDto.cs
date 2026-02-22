namespace BankSystemApi.Models
{
    public class TransferDto
    {
        public decimal Amount { get; set; }

        public string AccountNumberIn { get; set; }

        public string AccountNumberOut { get; set; }
    }
}

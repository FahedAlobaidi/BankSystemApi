using BankSystemApi.Models;

namespace BankSystemApi.TransactionsServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactions(string accountNumber);

        Task<TransactionDto> MakeDeposit(string accountNumber, decimal amount);

        Task<IEnumerable<TransactionDto>> Transfer(string accountNumberIn, decimal amount, string accountNumberOut);

        Task<TransactionDto> Withdrawal(string accountNumber,decimal amount);
    }
}

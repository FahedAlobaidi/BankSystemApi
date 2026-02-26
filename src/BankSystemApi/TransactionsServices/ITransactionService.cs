using BankSystemApi.Models;

namespace BankSystemApi.TransactionsServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactions(Guid clientId,string accountNumber);

        Task<TransactionDto> MakeDeposit(Guid clientId,string accountNumber, decimal amount);

        Task<IEnumerable<TransactionDto>> Transfer(Guid clientId,string accountNumberIn, decimal amount, string accountNumberOut);

        Task<TransactionDto> Withdrawal(Guid clientId,string accountNumber,decimal amount);
    }
}

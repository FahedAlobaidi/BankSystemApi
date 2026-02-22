using BankSystemApi.Entities;

namespace BankSystemApi.Services
{
    public interface ITransactionRepo
    {
        void AddTransaaction(Transaction transaction);

        Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid accountId);

        Task<bool> SaveChanges();
    }
}

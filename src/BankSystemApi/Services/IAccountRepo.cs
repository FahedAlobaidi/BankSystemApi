using BankSystemApi.Entities;

namespace BankSystemApi.Services
{
    public interface IAccountRepo
    {
        void AddAccount(Account account);

        Task<bool> IsAccountNumberExistAsync(string accountNumber);

        Task<Account?> GetAccountById(Guid accountId);

        Task<Account?> GetAccountByAccountNumber(string accountNumber);

        Task<IEnumerable<Account?>> GetAccountByClientIdAsync(Guid clientId);

        Task<bool> SaveChanges();
    }
}

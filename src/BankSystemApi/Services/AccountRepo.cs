using BankSystemApi.DbContexts;
using BankSystemApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly BankContext _context;

        public AccountRepo(BankContext context)
        {
            _context = context;
        }

        public void AddAccount(Account account)
        {
            _context.accounts.Add(account);
        }

        public async Task<Account?> GetAccountByAccountNumber(string accountNumber)
        {
            return await _context.accounts.Include(ac => ac.Client).FirstOrDefaultAsync(ac => ac.AccountNumber == accountNumber);
        }

        public async Task<IEnumerable<Account?>> GetAccountByClientIdAsync(Guid clientId)
        {
            //critical:here i include client bcs the account dto has information from the client
            //like firstName, lastName, ...etc 
            return await _context.accounts.Where(a => a.ClientId == clientId).Include(a=>a.Client).ToListAsync();
        }

        public async Task<Account?> GetAccountById(Guid accountId)
        {
            return await _context.accounts.Include(ac => ac.Client).FirstOrDefaultAsync(ac => ac.Id == accountId);
        }

        public async Task<bool> IsAccountNumberExistAsync(string accountNumber)
        {
            return await _context.accounts.AnyAsync(ac => ac.AccountNumber == accountNumber);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}

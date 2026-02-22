
using BankSystemApi.DbContexts;
using BankSystemApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.Services
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly BankContext _context;

        public TransactionRepo(BankContext context)
        {
            _context = context;
        }

        public void AddTransaaction(Transaction transaction)
        {
            _context.transactions.Add(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid accountId)
        {
            return await _context.transactions.Where(t => t.AccountId == accountId).OrderByDescending(t=>t.TransactionDate).ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

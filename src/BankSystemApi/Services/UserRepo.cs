using BankSystemApi.DbContexts;
using BankSystemApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly BankContext _context;

        public UserRepo(BankContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.users.Add(user);
        }

        public async Task<User?> GetUser(string email)
        {
            return await _context.users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithClientInfo(string email)
        {
            return await _context.users.Where(u => u.Email == email).Include(u => u.Client).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

using BankSystemApi.DbContexts;
using BankSystemApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.Services
{
    public class ClientRepository : IClientRepository
    {

        private readonly BankContext _context;

        public ClientRepository(BankContext context)
        {
            _context = context;
        }

        public void AddClientAsync(Client client)
        {
            _context.clients.Add(client);
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(int pageSize,int pageNumber)
        {
            return await _context.clients.AsNoTracking().OrderBy(c=>c.FirstName).Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
        }

        public async Task<Client?> GetClientAsync(Guid clientId)
        {
            return await _context.clients.AsNoTracking().Where(c => c.Id == clientId).FirstOrDefaultAsync();
        }

        

        public async Task<bool> IsClientExistAsync(Guid clientId)
        {
            return await _context.clients.AnyAsync(c => c.Id == clientId);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        //public void AddClientAsync(Client client)
        //{
        //    _context.clients.Add(client);
        //}

        //public async Task<IEnumerable<Client>> GetAllClientsAsync()
        //{
        //    return await _context.clients.ToListAsync();
        //}

        //public async Task<Client?> GetClientAsync(Guid clientId)
        //{
        //    return await _context.clients.Where(client => client.Id == clientId).FirstOrDefaultAsync();
        //}

        //public async Task<bool> IsClientExistAsync(Guid clientId)
        //{
        //    return await _context.clients.AnyAsync(client => client.Id == clientId);
        //}

        //public async Task<bool> SaveChanges()
        //{
        //    return (await _context.SaveChangesAsync() >= 0);
        //}
    }
}

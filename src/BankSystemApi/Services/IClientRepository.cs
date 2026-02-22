using BankSystemApi.Entities;

namespace BankSystemApi.Services
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(int pageSize,int pageNumber);

        Task<Client?> GetClientAsync(Guid clientId);

        Task<bool> IsClientExistAsync(Guid clientId);

        

        void AddClientAsync(Client client);

        Task<bool> SaveChanges();
    }
}

using BankSystemApi.Entities;

namespace BankSystemApi.Services
{
    public interface IUserRepo
    {
        void AddUser(User user);

        Task<bool> IsEmailExistAsync(string email);

        Task<User> GetUser(string email);

        Task<User?> GetUserWithClientInfo(string email);

        Task<bool> SaveChanges();
    }
}

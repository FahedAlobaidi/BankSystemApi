using BankSystemApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemApi.Services
{
    public interface IRegisterRepo
    {
        void AddRegistration(User user,Client client);

        Task<bool> IsEmailExistAsync(string email);

        Task<bool> SaveChanges();
    }
}
/*
- i will make a contract for register the user and this will save the data in the user and client table
- i will make a contract for checkin the email if it exist
- and the save changes method
*/
using AutoMapper;

namespace BankSystemApi.Profiles
{
    public class TransactionProfile:Profile
    {
        public TransactionProfile()
        {
            CreateMap<Entities.Transaction, Models.TransactionDto>();
        }
    }
}

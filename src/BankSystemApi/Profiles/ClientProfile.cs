using AutoMapper;

namespace BankSystemApi.Profiles
{
    public class ClientProfile:Profile
    {
        public ClientProfile()
        {
            CreateMap<Entities.Client, Models.ClientDto>();
            CreateMap<Models.ClientDto, Entities.Client>();

            CreateMap<Entities.Client, Models.ClientCreateDto>();
            CreateMap<Models.ClientCreateDto, Entities.Client>();

            CreateMap<Entities.Client, Models.ClientUpdateDto>();
            CreateMap<Models.ClientUpdateDto, Entities.Client>();
        }
    }
}

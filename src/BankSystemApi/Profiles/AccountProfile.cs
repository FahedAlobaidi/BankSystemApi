using AutoMapper;

namespace BankSystemApi.Profiles
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            
            CreateMap<Models.AccountCreateDto, Entities.Account>();
            CreateMap<Entities.Account, Models.AccountInfoDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Client.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Client.LastName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Client.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Client.Email));
        }
    }
}

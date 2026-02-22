using AutoMapper;

namespace BankSystemApi.Profiles
{
    public class RegistrationProfile:Profile
    {
        public RegistrationProfile()
        {
            CreateMap<Models.RegisterationDto, Entities.User>();
            CreateMap<Models.RegisterationDto, Entities.Client>();
        }
    }
}

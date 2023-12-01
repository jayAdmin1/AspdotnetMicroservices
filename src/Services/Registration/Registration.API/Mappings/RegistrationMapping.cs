using AutoMapper;
using Registration.API.Domains;
using Registration.API.ViewModels;

namespace Registration.API.Mappings
{
    public class RegistrationMapping : Profile
    {
        public RegistrationMapping()
        {
            CreateMap<User, UserDisplayModel>();
            CreateMap<UserUpdateModel, User>()
                .ForMember(x => x.Name, dest => dest.MapFrom(src => src.Name))
                .ForMember(x => x.MobileNo, dest => dest.MapFrom(src => src.MobileNo))
                .ForMember(x => x.Address, dest => dest.MapFrom(src => src.Address));
        }
    }
}

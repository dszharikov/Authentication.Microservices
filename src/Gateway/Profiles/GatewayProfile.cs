using AutoMapper;
using Gateway.Domain;
using Gateway.Dtos;

namespace Gateway.Profiles;

public class GatewayProfile : Profile
{
    public GatewayProfile()
    {
        // Source -> Target
        CreateMap<User, UserInfoDto>()
            .ForMember(dest => dest.PhoneLastNumbers,
                opt => opt.MapFrom(
                    src => src.PhoneNumber.Substring(src.PhoneNumber.Length - 4)));
        CreateMap<UserRegisteredDto, User>();
    }
}
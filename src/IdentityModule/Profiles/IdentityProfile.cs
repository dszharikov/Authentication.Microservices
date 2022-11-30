using AutoMapper;
using IdentityModule.Domain;
using IdentityModule.Dtos;
using IdentityModule.Infrastructure;

namespace IdentityModule.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        // Source -> Target
        CreateMap<PasswordCreatedDto, OneTimePassword>()
            .ForMember(dest => dest.Code,
                opt => opt.MapFrom(
                    src => src.Code))
            .ForMember(dest => dest.ExpiresAt,
                opt => opt.MapFrom(
                    src => src.ExpiresAt))
            .ForMember(dest => dest.NotBefore,
                opt => opt.MapFrom(
                    src => src.NotBefore))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(
                    src => true))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(
                    src => src.PhoneNumber));

        CreateMap<User, PublishUserDto>()
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(
                    src => src.PhoneNumber))
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(
                    src => src.Id))
            .ForMember(dest => dest.Event,
                opt => opt.MapFrom(
                    src => "User_Created"));
    }
}
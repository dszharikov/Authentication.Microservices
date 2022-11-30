using AutoMapper;
using Contacts.Domain;
using Contacts.Dto;
using Contacts.Dto.ContactLists;
using Contacts.Endpoints.ViewModel;

namespace Contacts.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        // Source --> Target
        CreateMap<Contact, ContactDetailedDto>();
        CreateMap<Contact, ContactLookupDto>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(
                    src => src.Id))
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(
                    src => src.Name));
        CreateMap<ContactCreateViewModel, Contact>();
    }
}
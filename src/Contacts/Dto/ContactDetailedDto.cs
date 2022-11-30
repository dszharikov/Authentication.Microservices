using Contacts.Domain.Base;

namespace Contacts.Dto;

public class ContactDetailedDto : Identity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
}
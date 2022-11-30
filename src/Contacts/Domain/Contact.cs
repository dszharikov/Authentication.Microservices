using Contacts.Domain.Base;

namespace Contacts.Domain;

public class Contact : Identity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
}
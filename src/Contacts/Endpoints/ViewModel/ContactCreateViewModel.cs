namespace Contacts.Endpoints.ViewModel;

public class ContactCreateViewModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
}
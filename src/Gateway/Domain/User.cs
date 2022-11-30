using Gateway.Domain.Base;

namespace Gateway.Domain;

public class User : Identity
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
}

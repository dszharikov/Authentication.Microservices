using Gateway.Domain.Base;

namespace Gateway.Dtos;

public class UserRegisteredDto : Identity
{
    public string PhoneNumber { get; set; }
}
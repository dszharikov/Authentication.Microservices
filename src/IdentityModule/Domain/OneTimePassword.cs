using IdentityModule.Domain.Base;

namespace IdentityModule.Domain;

public class OneTimePassword : Identity
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime NotBefore { get; set; }
    public bool IsActive { get; set; }
}
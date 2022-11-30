using IdentityModule.Domain.Base;

namespace IdentityModule.Infrastructure;

public class UserRole : Identity
{
    public User User { get; set; }
    public Role Role { get; set; }
}
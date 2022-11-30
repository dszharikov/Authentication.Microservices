using IdentityModule.Domain.Base;

namespace IdentityModule.Infrastructure;

public class Role : Identity
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
}
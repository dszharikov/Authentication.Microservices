using IdentityModule.Domain.Base;

namespace IdentityModule.Infrastructure;

/// <summary>
/// Represent person with login information (ApplicationUser)
/// </summary>
public class UserProfile : Auditable
{
    /// <summary>
    /// Account
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Application permission for policy-based authorization
    /// </summary>
    public List<AppPermission>? Permissions { get; set; }
}
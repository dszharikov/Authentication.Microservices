using IdentityModule.Domain.Base;

namespace IdentityModule.Infrastructure;
/// <summary>
/// User permission for application
/// </summary>
public class AppPermission : Auditable
{
    /// <summary>
    /// Application User profile identifier
    /// </summary>
    public UserProfile? UserProfile { get; set; }

    /// <summary>
    /// Authorize attribute policy name
    /// </summary>
    public string PolicyName { get; set; } = null!;

    /// <summary>
    /// Description for current permission
    /// </summary>
    public string Description { get; set; } = null!;
}
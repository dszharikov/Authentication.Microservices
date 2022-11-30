using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.Managers.RoleManager;

public interface IApplicationRoleManager
{
    Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken));
    Task<Role?> GetRoleByNormalizedNameAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken));
}
namespace IdentityModule.Infrastructure.Managers.UserRoleStore;

public interface IApplicationUserRoleStore
{
    Task<bool> IsInRoleAsync(User user, string normalizedRole,
        CancellationToken cancellationToken = default(CancellationToken));

    Task AddToRoleAsync(User user, string normalizedRole,
        CancellationToken cancellationToken = default(CancellationToken));
}
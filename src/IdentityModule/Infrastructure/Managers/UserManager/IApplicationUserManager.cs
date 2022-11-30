using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.Managers.UserManager;

public interface IApplicationUserManager
{
    Task<IdentityResult> AddToRoleAsync(User user, string roleName,
        CancellationToken cancellationToken = default(CancellationToken));
    Task<IdentityResult> CreateAsync(User user,
        CancellationToken cancellationToken = default(CancellationToken));

    Task<User?> GetUserByPhoneNumberAsync(string phoneNumber,
        CancellationToken cancellationToken = default(CancellationToken));
}
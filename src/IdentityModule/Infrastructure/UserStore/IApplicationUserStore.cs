using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.UserStore;

public interface IApplicationUserStore
{
    public Task<IdentityResult> CreateAsync(User user,
        CancellationToken cancellationToken = default(CancellationToken));
}
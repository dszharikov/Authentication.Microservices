using Calabonga.UnitOfWork;
using IdentityModule.Infrastructure.Managers.RoleManager;

namespace IdentityModule.Infrastructure.Managers.UserRoleStore;

public class ApplicationUserRoleStore : IApplicationUserRoleStore
{
    private readonly IApplicationRoleManager _roleManager;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public ApplicationUserRoleStore(IUnitOfWork<ApplicationDbContext> unitOfWork, IApplicationRoleManager roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task<bool> IsInRoleAsync(User user, string normalizedRole,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userRoleRepository = _unitOfWork.GetRepository<UserRole>();

        var role = await _roleManager.GetRoleByNormalizedNameAsync(normalizedRole, cancellationToken);
        if (role is null)
        {
            return false;
        }

        var userRole = await userRoleRepository.GetFirstOrDefaultAsync(
            predicate: ur => ur.Role == role && ur.User == user,
            disableTracking: false);

        return userRole is not null;
    }

    public async Task AddToRoleAsync(User user, string normalizedRole,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();


        var role = await _roleManager.GetRoleByNormalizedNameAsync(normalizedRole, cancellationToken);
        if (role is null)
        {
            return;
        }

        var userRole = new UserRole
        {
            User = user,
            Role = role
        };

        var userRoleRepository = _unitOfWork.GetRepository<UserRole>();
        await userRoleRepository.InsertAsync(userRole, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync();
    }
}
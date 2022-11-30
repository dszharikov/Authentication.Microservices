using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.Managers.RoleManager;

public class ApplicationRoleManager : IApplicationRoleManager
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public ApplicationRoleManager(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IdentityResult> CreateAsync(Role role,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        var roleRepository = _unitOfWork.GetRepository<Role>();

        // check if already there is a role in repository
        var roleResult = await GetRoleByNormalizedNameAsync(role.NormalizedName, cancellationToken);
        if (roleResult is not null)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "This role is already exists in a database" });
        }

        await roleRepository.InsertAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return _unitOfWork.LastSaveChangesResult.IsOk ? IdentityResult.Success : IdentityResult.Failed();
    }

    public async Task<Role?> GetRoleByNormalizedNameAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var roleRepository = _unitOfWork.GetRepository<Role>();

        return await roleRepository.GetFirstOrDefaultAsync(
            predicate: r => r.NormalizedName == roleName,
            disableTracking: false);
    }
}
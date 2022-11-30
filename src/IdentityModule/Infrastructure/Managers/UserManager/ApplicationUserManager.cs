using Calabonga.UnitOfWork;
using IdentityModule.Infrastructure.Managers.RoleManager;
using IdentityModule.Infrastructure.Managers.UserRoleStore;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.Managers.UserManager;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly IApplicationUserRoleStore _userRoleStore;

    public ApplicationUserManager(IApplicationUserRoleStore userRoleStore, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _userRoleStore = userRoleStore;
        _unitOfWork = unitOfWork;
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var normalizedRole = roleName.ToUpper();
        if (await _userRoleStore.IsInRoleAsync(user, normalizedRole, cancellationToken))
        {
            return IdentityResult.Failed(new IdentityError { Description = "User already in role" });
        }

        await _userRoleStore.AddToRoleAsync(user, normalizedRole, cancellationToken);
        
        _unitOfWork.GetRepository<User>().Update(user);

        await _unitOfWork.SaveChangesAsync();
        return _unitOfWork.LastSaveChangesResult.IsOk ? IdentityResult.Success : IdentityResult.Failed();
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        await _unitOfWork.GetRepository<User>().InsertAsync(user, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync();
        return _unitOfWork.LastSaveChangesResult.IsOk ? IdentityResult.Success : IdentityResult.Failed();
    }

    public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _unitOfWork.GetRepository<User>()
            .GetFirstOrDefaultAsync(predicate: user => user.PhoneNumber == phoneNumber, disableTracking: false);
    }
}
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Infrastructure.UserStore;

public class ApplicationUserStore : IApplicationUserStore
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public ApplicationUserStore(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        // check if there are links between AppPermissions and UserProfile
        user.UserProfile.Permissions?.ForEach(p => p.UserProfile = user.UserProfile);

        var userRepository = _unitOfWork.GetRepository<User>();

        await userRepository.InsertAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return _unitOfWork.LastSaveChangesResult.IsOk ? IdentityResult.Success : IdentityResult.Failed();
    }
}
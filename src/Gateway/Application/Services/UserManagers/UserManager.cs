using Calabonga.UnitOfWork;
using Gateway.Domain;
using Gateway.Infrastructure;

namespace Gateway.Application.Services.UserManagers;

public class UserManager : IUserManager
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public UserManager(
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _unitOfWork.GetRepository<User>()
            .GetFirstOrDefaultAsync(predicate: user => user.Id == id, disableTracking: false);
    }

    public async Task<int> UpdateUserInfo(User user, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _unitOfWork.GetRepository<User>().Update(user);

        return await _unitOfWork.SaveChangesAsync();
    }
}
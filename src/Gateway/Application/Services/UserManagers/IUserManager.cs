using Gateway.Domain;

namespace Gateway.Application.Services.UserManagers;

public interface IUserManager
{
    public Task<User?> GetUser(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    public Task<int> UpdateUserInfo(User user, CancellationToken cancellationToken = default(CancellationToken));
}
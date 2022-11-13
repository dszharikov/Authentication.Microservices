using Calabonga.UnitOfWork;
using IdentityModule.Domain;
using IdentityModule.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Web.Application.Services.PasswordVerificator;

public class PasswordVerificator : IPasswordVerificator
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    public PasswordVerificator(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PasswordVerificationResult> CheckOneTimePassword(string phoneNumber, string password, bool once = true)
    {
        var passwordRepository = _unitOfWork.GetRepository<OneTimePassword>();
        var dateTimeNow = DateTime.Now;
        var passwordFromDb = await passwordRepository.GetFirstOrDefaultAsync(
            predicate: password =>
            password.PhoneNumber == phoneNumber
            && password.IsActive
            && password.ExpiresAt > dateTimeNow
            && password.NotBefore < dateTimeNow, disableTracking: !once);

        if (passwordFromDb is not null)
        {
            if (once)
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                passwordFromDb.IsActive = false;
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            return PasswordVerificationResult.Success;
        }
        return PasswordVerificationResult.Failed;
    }
}

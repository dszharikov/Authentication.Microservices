using Calabonga.UnitOfWork;
using IdentityModule.Domain;
using IdentityModule.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Applications.Services.PasswordValidator;

public class PasswordValidator : IPasswordValidator
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public PasswordValidator(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PasswordVerificationResult> ValidateOneTimePassword(string phoneNumber, string password,
        bool once = true)
    {
        var passwordRepository = _unitOfWork.GetRepository<OneTimePassword>();
        var dateTimeNow = DateTime.Now;

        Console.WriteLine(passwordRepository.Count());

        var passwordFromDb = await passwordRepository.GetFirstOrDefaultAsync(
            predicate: p =>
                p.PhoneNumber == phoneNumber
                && p.Code == password
                && p.IsActive
                && p.ExpiresAt > dateTimeNow
                && p.NotBefore < dateTimeNow, disableTracking: !once);

        if (passwordFromDb is not null)
        {
            if (once)
            {
                //await using var transaction = await _unitOfWork.BeginTransactionAsync();
                passwordFromDb.IsActive = false;
                await _unitOfWork.SaveChangesAsync();
                //await transaction.CommitAsync();
            }

            return PasswordVerificationResult.Success;
        }

        return PasswordVerificationResult.Failed;
    }
}
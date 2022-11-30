using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Applications.Services.PasswordValidator;

public interface IPasswordValidator
{
    Task<PasswordVerificationResult> ValidateOneTimePassword(string phoneNumber, string password, bool once = true);
}
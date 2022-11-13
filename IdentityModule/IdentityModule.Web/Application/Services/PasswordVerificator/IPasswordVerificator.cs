using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Web.Application.Services.PasswordVerificator;

public interface IPasswordVerificator
{
    Task<PasswordVerificationResult> CheckOneTimePassword(string phoneNumber, string password, bool once = true);
}

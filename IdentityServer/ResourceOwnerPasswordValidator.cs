using Duende.IdentityServer.Validation;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ApplicationDbContext _context;

        public ResourceOwnerPasswordValidator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var phoneNumber = context.UserName;
            var password = context.Password;

            var dateTimeNow = DateTime.Now;

            var user = await _context.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);
            var dbPassword = await _context.Passwords
                .FirstOrDefaultAsync(password => password.PhoneNumber == phoneNumber
                && password.IsActive
                && password.NotBefore < dateTimeNow
                && password.ExpiresAt > dateTimeNow);

            var userId = "null";

            if (user is not null)
            {
                userId = user.Id;
            }

            if (dbPassword is not null)
            {
                context.Result = new GrantValidationResult(
                subject: userId,
                authenticationMethod: "otpAuthentication");

                dbPassword.IsActive = false;
                _context.Passwords.Update(dbPassword);
                _context.SaveChanges();

                return;
            }
            else
            {
                // password is not equal;
                return;
            }
        }
    }
}

using IdentityModule.Domain;
using IdentityModule.Domain.Base.AppData;
using IdentityModule.Infrastructure.Managers.RoleManager;
using IdentityModule.Infrastructure.Managers.UserManager;
using IdentityModule.Infrastructure.UserStore;
using Microsoft.EntityFrameworkCore;

namespace IdentityModule.Infrastructure;

/// <summary>
/// Database Initializer
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Seeds one default users to database for demo purposes only
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static async void SeedUsers(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        /*if (environment.IsProduction())
        {
            await context!.Database.EnsureCreatedAsync();
            var pending = await context.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                await context!.Database.MigrateAsync();
            }
        }*/


        var roles = AppData.Roles.ToArray();

        foreach (var role in roles)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<IApplicationRoleManager>();
            if (!context!.Roles.Any(r => r.Name == role))
            {
                await roleManager.CreateAsync(new Role { Name = role, NormalizedName = role.ToUpper() });
            }
        }

        #region developer

        var developer1 = new User
        {
            PhoneNumber = "+79000000000",
            UserProfile = new UserProfile
            {
                CreatedAt = DateTime.Now.ToUniversalTime(),
                CreatedBy = "SEED",
                Permissions = new List<AppPermission>
                {
                    new()
                    {
                        CreatedAt = DateTime.Now.ToUniversalTime(),
                        CreatedBy = "SEED",
                        PolicyName = "EventItems:UserRoles:View",
                        Description = "Access policy for EventItems controller user view"
                    }
                }
            }
        };

        if (!context!.Users.Any(u => u.PhoneNumber == developer1.PhoneNumber))
        {
            var userStore = scope.ServiceProvider.GetRequiredService<IApplicationUserStore>();
            var result = await userStore.CreateAsync(developer1);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Cannot create account");
            }

            var userManager = scope.ServiceProvider.GetService<IApplicationUserManager>();
            foreach (var role in roles)
            {
                var roleAdded = await userManager!.AddToRoleAsync(developer1, role);
                if (roleAdded.Succeeded)
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        #endregion

        /*#region OTP

        var oneTimePassword = new OneTimePassword
        {
            NotBefore = DateTime.Now.ToUniversalTime(),
            ExpiresAt = DateTime.Now.AddMinutes(30).ToUniversalTime(),
            PhoneNumber = "+79000000001",
            Code = "5536",
            IsActive = true
        };

        if (!context!.OneTimePasswords.Any(password => password.PhoneNumber == oneTimePassword.PhoneNumber))
        {
            context.OneTimePasswords.Add(oneTimePassword);
            await context.SaveChangesAsync();
        }

        #endregion*/

        await context.SaveChangesAsync();
    }
}
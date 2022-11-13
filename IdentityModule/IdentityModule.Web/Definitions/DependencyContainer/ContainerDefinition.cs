using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Web.Application.Services.Account;
using IdentityModule.Web.Application.Services.PasswordVerificator;
using IdentityModule.Web.Definitions.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Web.Definitions.DependencyContainer
{
    /// <summary>
    /// Dependency container definition
    /// </summary>
    public class ContainerDefinition : AppDefinition
    {
        public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPasswordVerificator, PasswordVerificator>();
            services.AddTransient<ApplicationUserClaimsPrincipalFactory>();
        }
    }
}
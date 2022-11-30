using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Application.Services.EventProcessing;
using IdentityModule.Applications.Services.Account;
using IdentityModule.Applications.Services.PasswordValidator;
using IdentityModule.AsyncDataServices;
using IdentityModule.AsyncDataServices.MessageBusClients;
using IdentityModule.Definitions.OpenIddict;
using IdentityModule.Infrastructure.Managers.RoleManager;
using IdentityModule.Infrastructure.Managers.UserManager;
using IdentityModule.Infrastructure.Managers.UserRoleStore;
using IdentityModule.Infrastructure.UserStore;
using Password.AsyncDataServices;

namespace IdentityModule.Definitions.DependencyContainer;

/// <summary>
/// Dependency container definition
/// </summary>
public class ContainerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddTransient<IApplicationUserStore, ApplicationUserStore>();
        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
        services.AddTransient<IApplicationRoleManager, ApplicationRoleManager>();
        services.AddTransient<IApplicationUserRoleStore, ApplicationUserRoleStore>();
        
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IPasswordValidator, PasswordValidator>();
        services.AddTransient<ApplicationUserClaimsPrincipalFactory>();

        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<MessageBusSubscriber>();
        
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

    }
}
using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.UnitOfWork;
using IdentityModule.Infrastructure;

namespace IdentityModule.Definitions.UnitOfWork;

/// <summary>
/// Unit of Work registration as application definition
/// </summary>
public class UnitOfWorkDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        => services.AddUnitOfWork<ApplicationDbContext>();
}
using Calabonga.AspNetCore.AppDefinitions;
using Contacts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Definitions.DbContext;

/// <summary>
/// ASP.NET Core services registration and configurations
/// </summary>
public class DbContextDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            config.UseInMemoryDatabase("Contacts DEMO");
        });
    }
}
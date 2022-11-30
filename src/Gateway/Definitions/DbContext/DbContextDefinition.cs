using Calabonga.AspNetCore.AppDefinitions;
using Gateway.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Definitions.DbContext;

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
            //var environment = builder.Environment;

            config.UseInMemoryDatabase("Gateway DEMO");
            /*if (environment.IsDevelopment())
            {
                //config.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
                config.UseInMemoryDatabase("DEMO-PURPOSES-ONLY");
            }

            if (environment.IsProduction())
            {
                //config.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
            }*/
        });
    }
}
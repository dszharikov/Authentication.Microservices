using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace IdentityModule.Definitions.DbContext;

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
            var environment = builder.Environment;

            if (environment.IsDevelopment())
            {
                //config.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
                config.UseInMemoryDatabase("DEMO-PURPOSES-ONLY");

            }
            if (environment.IsProduction())
            {
                //config.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));

                config.UseInMemoryDatabase("DEMO-PURPOSES-ONLY");
            }

            // uncomment line below to use UseSqlServer(). Don't forget setup connection string in appSettings.json

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need to replace the default OpenIddict entities.
            config.UseOpenIddict<Guid>();
        });


        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            // configure more options if you need
        });

    }
}
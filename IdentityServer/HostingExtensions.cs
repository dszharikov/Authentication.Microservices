using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var connectionString = configuration.GetConnectionString("ApplicationDbContext");

        var migrationsAssembly = typeof(Config).Assembly.GetName().Name;

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
        //.AddAspNetIdentity<IdentityUser>();


        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        //app.UseStaticFiles();
        //app.UseRouting();

        app.UseIdentityServer();

        app.MapGet("/hello", () => "hello");

        /*app.MapGet("/seed", () =>
        {
            Log.Information("Seeding database...");
            SeedData.EnsureSeedData(app);
            Log.Information("Done seeding database. Exiting.");
        });*/

        // uncomment if you want to add a UI
        //app.UseAuthorization();
        //app.MapRazorPages().RequireAuthorization();

        return app;
    }
}

using Calabonga.AspNetCore.AppDefinitions;

namespace Gateway.Definitions.Common;

/// <summary>
/// AspNetCore common configuration
/// </summary>
public class CommonDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHttpContextAccessor();
        services.AddResponseCaching();
        services.AddMemoryCache();
    }

    /// <summary>
    /// Configure application for current application
    /// </summary>
    /// <param name="app"></param>
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
}
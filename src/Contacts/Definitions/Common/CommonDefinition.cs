using Calabonga.AspNetCore.AppDefinitions;

namespace Contacts.Definitions.Common;

public class CommonDefinition : AppDefinition
{
    
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHttpContextAccessor();
        services.AddResponseCaching();
        services.AddMemoryCache();
    }
    
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
}
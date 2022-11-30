using Calabonga.AspNetCore.AppDefinitions;

namespace Contacts.Definitions.Mapping;

/// <summary>
/// Register Automapper as MicroserviceDefinition
/// </summary>
public class AutomapperDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current microservice
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        => services.AddAutoMapper(typeof(Program));

}
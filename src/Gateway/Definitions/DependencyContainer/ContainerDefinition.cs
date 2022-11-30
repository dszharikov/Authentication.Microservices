using Calabonga.AspNetCore.AppDefinitions;
using Gateway.Application.Services.EventProcessing;
using Gateway.Application.Services.PasswordService;
using Gateway.AsyncDataServices;

namespace Gateway.Definitions.DependencyContainer;

/// <summary>
/// Dependency container definition
/// </summary>
public class ContainerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHttpClient<IPasswordService, PasswordService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["HttpPassword"] + "/generate"!);
        });
        
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<MessageBusSubscriber>();
    }
}
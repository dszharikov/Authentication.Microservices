using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Definitions.Authorization;

public class AuthorizationDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = builder.Configuration["Authority"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "api");
            });
        });
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseAuthorization();
    }
}
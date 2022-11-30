using System.Security.Claims;
using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Applications.Services.Account;
using IdentityModule.Infrastructure.Managers.UserManager;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace IdentityModule.Endpoints.Token;

public class TokenDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapPost("~/connect/token", TokenAsync).ExcludeFromDescription();
    }

    private async Task<IResult> TokenAsync(
        HttpContext httpContext,
        IAccountService accountService)
    {
        var request = httpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
        {
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Subject or sub is a required field, we use the client id as the subject identifier here.
            identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId!);
            identity.AddClaim(OpenIddictConstants.Claims.ClientId, request.ClientId!);


            // Don't forget to add destination otherwise it won't be added to the access token.
            identity.AddClaim(OpenIddictConstants.Claims.Scope, request.Scope!,
                OpenIddictConstants.Destinations.AccessToken);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            claimsPrincipal.SetScopes(request.GetScopes());
            return Results.SignIn(claimsPrincipal, new AuthenticationProperties(),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsPasswordGrantType())
        {
            if (request.Username is null || request.Password is null)
            {
                return Results.Problem("Please, enter username and password correctly");
            }
            return await accountService.GetTokenAsync(request.Username, request.Password, request, httpContext.RequestAborted);
        }

        return Results.Problem("The specified grant type is not supported.");
    }
}
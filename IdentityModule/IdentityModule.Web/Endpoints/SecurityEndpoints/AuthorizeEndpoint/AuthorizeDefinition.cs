using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Infrastructure;
using IdentityModule.Web.Application.Services.Account;
using IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace IdentityModule.Web.Endpoints.SecurityEndpoints.AuthorizeEndpoint
{
    public class AuthorizeDefinition : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            app.MapPost("~/connect/token", TokenAsync).ExcludeFromDescription();
        }

        /*private async Task<IResult> AuthorizeAsync([FromServices] IMediator mediator, AuthorizeViewModel model, HttpContext context)
            => await mediator.Send(new AuthorizeAccountRequest(model), context.RequestAborted);*/

        private async Task<IResult> TokenAsync(
            IMediator mediator,
            HttpContext httpContext,
            IOpenIddictScopeManager manager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountService accountService)
        {
            var request = httpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Subject or sub is a required field, we use the client id as the subject identifier here.
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId!);
                identity.AddClaim(OpenIddictConstants.Claims.ClientId, request.ClientId!);


                // Don't forget to add destination otherwise it won't be added to the access token.
                identity.AddClaim(OpenIddictConstants.Claims.Scope, request.Scope!, OpenIddictConstants.Destinations.AccessToken);

                var claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
                return Results.SignIn(claimsPrincipal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            if (request.IsPasswordGrantType())
            {
                var authorizeViewModel = new AuthorizeViewModel
                {
                    PhoneNumber = request.Username,
                    Password = request.Password
                };

                var resultMediator = await accountService.AuthorizeAsync(authorizeViewModel, httpContext.RequestAborted);
                return resultMediator is IResult ? resultMediator : Results.Problem("The specified grant type is not supported.");
            }
            return Results.Problem("The specified grant type is not supported.");
        }
    }
}
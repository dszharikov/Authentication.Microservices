using Calabonga.AspNetCore.AppDefinitions;
using IdentityModule.Web.Definitions.OpenIddict;
using IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.Queries;
using IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace IdentityModule.Web.Endpoints.ProfileEndpoints.FillUserDataEndpoint;

public class FillUserDataDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapPost("~/profile/filluserdata", FillUserDataAsync).ExcludeFromDescription();
    }

    [Authorize(AuthenticationSchemes = AuthData.AuthSchemes, Policy = "Identity:FillUserData")]
    private async Task<IResult> FillUserDataAsync([FromServices] IMediator mediator, FillUserDataViewModel model, HttpContext context)
        => await mediator.Send(new FillUserDataRequest(model), context.RequestAborted);
}
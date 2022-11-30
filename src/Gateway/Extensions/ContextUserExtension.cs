using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Gateway.Extensions;

public static class ContextUserExtension
{
    public static Guid GetId(this ClaimsPrincipal principal) => Guid.Parse(principal.GetClaim(OpenIddictConstants.Claims.Subject) ?? string.Empty);
}
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Validation.AspNetCore;

namespace IdentityModule.Web.Definitions.OpenIddict
{
    public static class AuthData
    {
        public const string AuthSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    }
}
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
        new ApiScope(name: "api", displayName: "ContactsInfo"),
        new ApiScope(name: "otpApi", displayName: "otpApi"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
        // machine to machine client (from quickstart 1)
        new Client
        {
            ClientId = "mobileClient",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            // scopes that client has access to
            AllowedScopes = { "otpApi" }
        },

        new Client
        {
            ClientId = "userMobileClient",
            RequireClientSecret = false,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AccessTokenLifetime = 3600,

            AllowedScopes = { "api" }
        }
        };
}
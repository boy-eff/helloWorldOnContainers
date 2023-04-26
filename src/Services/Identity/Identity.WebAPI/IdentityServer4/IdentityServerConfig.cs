using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.WebAPI.IdentityServer4;

public static class IdentityServerConfig
{
    private const string WordsApiScopeName = "words";
    private const string AchievementsApiScopeName = "achievements";
    private const string RoleApiScopeName = "role";
    private const string ClientName = "client";
    private const string ClientSecret = "secret";

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new(WordsApiScopeName),
            new(AchievementsApiScopeName),
            new(IdentityServerConstants.LocalApi.ScopeName),
        };
    
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[] {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    
    public static IEnumerable<ApiResource> ApiResources => 
        new List<ApiResource>
        {
            new(IdentityServerConstants.LocalApi.ScopeName),
            new(RoleApiScopeName),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = ClientName,

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret(ClientSecret.Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { WordsApiScopeName, AchievementsApiScopeName, RoleApiScopeName, IdentityServerConstants.LocalApi.ScopeName },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60,
            }
        };
}
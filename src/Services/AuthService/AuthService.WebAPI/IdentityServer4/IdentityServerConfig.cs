using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace AuthService.WebAPI.IdentityServer4
{
    public class IdentityServerConfig
    {
        private const string _wordsApiScopeName = "words";

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(_wordsApiScopeName)
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                //TO-DO
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "words" }
                }
            };
    }
}
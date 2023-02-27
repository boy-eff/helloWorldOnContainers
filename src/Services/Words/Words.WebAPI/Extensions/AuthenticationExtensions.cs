using System.Security.Claims;
using IdentityServer4.AccessTokenValidation;

namespace Words.WebAPI.Extensions;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = config["IdentityServer:IssuerUri"];
                options.RequireHttpsMetadata = false;
                options.NameClaimType = ClaimTypes.Name;
                options.RoleClaimType = ClaimTypes.Role;
            });
    }
}
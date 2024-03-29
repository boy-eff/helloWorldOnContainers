﻿using IdentityServer4.AccessTokenValidation;
using Shared.Constants;

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
                options.RoleClaimType = ClaimNames.RoleClaimName;
            });
    }
}
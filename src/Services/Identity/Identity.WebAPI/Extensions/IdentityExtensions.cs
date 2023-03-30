using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Identity.WebAPI.IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;

namespace Identity.WebAPI.Extensions;

public static class IdentityExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AuthDbContext>()
            .AddRoles<IdentityRole<int>>();
    }

    public static void ConfigureIdentityServer(this IServiceCollection services, ConfigurationManager config) 
    {
        services.AddIdentityServer(options =>
            {
                options.IssuerUri = config["IdentityServerSettings:IssuerUri"];
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
            .AddProfileService<ProfileService>();
        
        services.AddLocalApiAuthentication();
    }
    
    public static void ConfigureAuthentication(this IServiceCollection services) 
    {
        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimNames.RoleClaimName
                };
            });
    }
    
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminOnly, policy =>
            {
                policy.RequireRole(Roles.AdminRole.Name);
            });
        });
    }
}
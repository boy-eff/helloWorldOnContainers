using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Identity.WebAPI.IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

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
            .AddProfileService<ProfileService>();
    }
    
    public static void ConfigureAuthentication(this IServiceCollection services) 
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}
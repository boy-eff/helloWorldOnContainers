using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.WebAPI.IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AuthService.WebAPI.Extensions;

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
        }).AddEntityFrameworkStores<AuthDbContext>();
    }

    public static void ConfigureIdentityServer(this IServiceCollection services) 
    {
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryClients(IdentityServerConfig.Clients);
    }
    
    public static void ConfigureAuthentication(this IServiceCollection services) 
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Data.Entities;
using AuthService.Infrastructure.Data;
using AuthService.WebAPI.IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, string? connectionString) 
        {
            if (connectionString is null) 
            {
                Debug.Print("Connection string is not specified");
                return;
            }
            services.AddDbContext<AuthDbContext>(options => {
                options.UseNpgsql(connectionString);
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
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
    }
}
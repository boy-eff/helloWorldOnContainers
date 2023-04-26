using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Shared.Extensions;

public static class CorsExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CorsConfigurationOptions>(config.GetSection(CorsConfigurationOptions.CorsSection));
        var corsOptions = config.GetSection(CorsConfigurationOptions.CorsSection).Get<CorsConfigurationOptions>();
        services.AddCors(options =>
        {
            options.AddPolicy(corsOptions.PolicyName,
                builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(corsOptions.Origins);
                });
        });
    }
}
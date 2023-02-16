using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static void ConfigureDatabaseConnection(this IServiceCollection services, ConfigurationManager config)
    {
        var connectionString = config.GetConnectionString("Default");
        services.AddDbContext<AuthDbContext>(options => {
            options.UseNpgsql(connectionString);
        });
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();

        var dbContext = services.ServiceProvider.GetService<AuthDbContext>();
        dbContext?.Database.Migrate();
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using Words.DataAccess.Models;

namespace Words.DataAccess.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();

        var dbContext = services.ServiceProvider.GetService<WordsDbContext>();
        dbContext?.Database.Migrate();
    }
}
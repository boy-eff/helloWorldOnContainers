using Achievements.Domain;
using Microsoft.EntityFrameworkCore;

namespace Achievements.WebAPI.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();

        var dbContext = services.ServiceProvider.GetService<AchievementsDbContext>();
        await dbContext?.Database.MigrateAsync();
        if (!dbContext.Achievements.Any())
        {
            await dbContext.Achievements.AddRangeAsync(SeedData.Achievements);
            await dbContext.SaveChangesAsync();
        }
    }
}
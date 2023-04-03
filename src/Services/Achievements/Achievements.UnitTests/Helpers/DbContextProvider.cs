using Achievements.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Achievements.UnitTests.Helpers;

public static class DbContextProvider
{
    public static AchievementsDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;
        return new AchievementsDbContext(options, new ConfigurationManager());
    }
}
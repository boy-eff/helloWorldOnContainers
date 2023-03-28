using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Words.DataAccess;

namespace Words.UnitTests.Helpers;

public static class DbContextProvider
{
    public static WordsDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;
        return new WordsDbContext(options, new ConfigurationManager());
    }
}
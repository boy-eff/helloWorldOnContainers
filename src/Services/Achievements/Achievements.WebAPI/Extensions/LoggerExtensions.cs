using Serilog;

namespace Achievements.WebAPI.Extensions;

public static class LoggerExtensions
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .Enrich.WithProperty("Service", builder.Configuration["ServiceName"])
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(logger);
    }
}
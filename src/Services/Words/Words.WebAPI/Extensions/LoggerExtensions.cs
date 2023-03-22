using Serilog;
using Serilog.Events;

namespace Words.WebAPI.Extensions;

public static class LoggerExtensions
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(logger);
    }
}
using Serilog;
using Serilog.Events;

namespace Words.WebAPI.Extensions;

public static class LoggerExtensions
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var loggerConfiguration = new LoggerConfiguration();
        loggerConfiguration.WriteTo.Console();
        
        if (builder.Environment.EnvironmentName == "Docker")
        {
            loggerConfiguration.WriteTo.Seq(builder.Configuration["Logging:SeqApi"], LogEventLevel.Information);
        }

        var logger = loggerConfiguration.CreateLogger();

        builder.Logging.AddSerilog(logger);
    }
}
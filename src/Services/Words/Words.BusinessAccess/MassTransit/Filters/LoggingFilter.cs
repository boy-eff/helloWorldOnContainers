using MassTransit;
using Microsoft.Extensions.Logging;

namespace Words.BusinessAccess.MassTransit.Filters;

public class LoggingFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly ILogger<LoggingFilter<T>> _logger;

    public LoggingFilter(ILogger<LoggingFilter<T>> logger)
    {
        _logger = logger;
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        _logger.LogInformation("Received message: {@message}", context);
        await next.Send(context);
        _logger.LogInformation("Processed message: {@message}", context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("logging");
    }
}
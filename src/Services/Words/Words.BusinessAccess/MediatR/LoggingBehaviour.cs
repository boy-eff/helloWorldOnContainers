using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Words.BusinessAccess.MediatR;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        _logger.LogInformation($"Handling {requestType.Name}");
        IList<PropertyInfo> props = new List<PropertyInfo>(requestType.GetProperties());
        foreach (var prop in props)
        {
            var propValue = prop.GetValue(request, null);
            _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
        }
        
        var response = await next();
        _logger.LogInformation($"Handled {requestType}");
        return response;
    }
}
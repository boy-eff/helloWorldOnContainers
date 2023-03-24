using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Words.BusinessAccess.MediatR.Middleware;

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
        IList<PropertyInfo> props = new List<PropertyInfo>(requestType.GetProperties());
        var propsWithValues = props
            .Select(prop => new { Name = prop.Name, Value = prop.GetValue(request, null) });
        _logger.LogInformation("Handling {requestName} with properties {@props}", requestType.Name, propsWithValues);

        var response = await next();
        
        _logger.LogInformation($"Handled {requestType}");
        return response;
    }
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Words.BusinessAccess.Extensions;

namespace Words.BusinessAccess.MediatR.Middleware;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            validator.ValidateAndThrowCustomException(request, _logger);
        }
        return await next();
    }
}
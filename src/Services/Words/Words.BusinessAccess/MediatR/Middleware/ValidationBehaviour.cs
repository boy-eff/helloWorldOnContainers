using FluentValidation;
using MediatR;

namespace Words.BusinessAccess.MediatR.Middleware;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        }
        return await next();
    }
}
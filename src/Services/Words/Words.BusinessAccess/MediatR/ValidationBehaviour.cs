using FluentValidation;
using MediatR;

namespace Words.BusinessAccess.MediatR;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehaviour(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return await next();
    }
}
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace Words.BusinessAccess.Extensions;

public static class FluentValidationExtensions
{
    public static void ValidateAndThrowCustomException<T>(this IValidator<T> validator, T instance, ILogger logger = null)
    {
        var res = validator.Validate(instance);
        
        if (!res.IsValid)
        {
            logger?.LogInformation("Validation failed with errors: {ValidationErrors}", res.Errors);
            throw new CustomValidationException(res.Errors);
        }
    }
}
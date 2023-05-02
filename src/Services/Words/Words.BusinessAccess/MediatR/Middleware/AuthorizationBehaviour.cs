using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using Words.BusinessAccess.Extensions;

namespace Words.BusinessAccess.MediatR.Middleware;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationBehaviour<TRequest, TResponse>> _logger;

    public AuthorizationBehaviour(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizationBehaviour<TRequest, TResponse>> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        if (userId is null or 0)
        {
            _logger.LogInformation("Authorization failed: User with id {UserId} was not found", userId);
            throw new AuthorizationException($"User with id {userId} was not found");
        }

        var response = await next();
        return response;
    }
}
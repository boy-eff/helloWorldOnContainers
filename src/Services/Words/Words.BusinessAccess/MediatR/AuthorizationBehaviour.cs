using MediatR;
using Microsoft.AspNetCore.Http;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;

namespace Words.BusinessAccess.MediatR;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        if (userId is null or 0)
        {
            throw new AuthorizationException("User id was not found");
        }

        var response = await next();
        return response;
    }
}
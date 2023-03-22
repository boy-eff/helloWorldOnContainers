using MediatR;
using Words.BusinessAccess.MediatR;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;
using Words.BusinessAccess.MediatR.Middleware;

namespace Words.WebAPI.Extensions;

public static class MediatRExtensions
{
    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetWordCollectionsQuery).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}
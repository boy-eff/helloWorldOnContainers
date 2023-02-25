using MediatR;
using Words.BusinessAccess.Features.Collections.Queries.Get;
using Words.BusinessAccess.MediatR;

namespace Words.WebAPI.Extensions;

public static class MediatRExtensions
{
    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetWordCollectionsQuery).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}
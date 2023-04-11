using Shared.Constants;

namespace Words.WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ModeratorOnly, policy =>
            {
                policy.RequireRole(Roles.ModeratorRole.Name);
            });
        });
    }
}
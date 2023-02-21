namespace Words.WebAPI.Extensions;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = config["IdentityServer:IssuerUri"];
                options.RequireHttpsMetadata = false;
            });
    }
}
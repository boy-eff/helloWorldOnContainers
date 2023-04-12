using Words.BusinessAccess.Options;

namespace Words.WebAPI.Extensions;

public static class CloudinaryExtensions
{
    public static void ConfigureCloudinaryOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CloudinaryConfigurationOptions>(
            config.GetSection(CloudinaryConfigurationOptions.CloudinarySection));
    }
}
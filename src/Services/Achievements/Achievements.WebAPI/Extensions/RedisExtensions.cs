namespace Achievements.WebAPI.Extensions;

public static class RedisExtensions
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config["RedisCacheUrl"];
            options.InstanceName = "local";
        });
    }
}
using StackExchange.Redis;

namespace Words.WebAPI.Extensions;

public static class RedisExtensions
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration config)
    {
        var redisCacheUrl = config["Redis:Url"];
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheUrl;
            options.InstanceName = "local";
        });
    }
}
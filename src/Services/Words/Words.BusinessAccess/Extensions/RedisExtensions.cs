using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class RedisExtensions
{
    private const string WordCollectionName = nameof(WordCollection);
    public static async Task SetWordCollectionAsync(this IDistributedCache cache, WordCollection wordCollection,
        TimeSpan? slidingExpiration = null)
    {
        var key = WordCollectionName + wordCollection.Id;
        await SetAsync(cache, key, wordCollection, slidingExpiration);
    }
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan? slidingExpiration = null)
    {
        var options = new DistributedCacheEntryOptions();
        
        if (slidingExpiration is not null)
        {
            options.SlidingExpiration = slidingExpiration;
        }
        
        return SetAsync(cache, key, value, options);
    }
    
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, GetJsonSerializerOptions()));
        return cache.SetAsync(key, bytes, options);
    }
    
    public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;
        if (val == null) return false;
        value = JsonSerializer.Deserialize<T>(val, GetJsonSerializerOptions());
        return true;
    }

    public static bool TryGetWordCollection(this IDistributedCache cache, int id, out WordCollection wordCollection)
    {
        var key = WordCollectionName + id;
        return TryGetValue(cache, key, out wordCollection);
    }
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.Preserve
        };
    }
}
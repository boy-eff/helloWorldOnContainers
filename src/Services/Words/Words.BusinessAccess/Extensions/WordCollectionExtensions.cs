using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Words.BusinessAccess.Helpers;
using Words.BusinessAccess.Options;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class WordCollectionExtensions
{
    public static async Task AddToCacheAsync(this WordCollection wordCollection, IDistributedCache cache,
        IOptions<WordsRedisCacheOptions> options)
    {
        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(wordCollection.Id);
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(options.Value.SlidingExpirationTimeInMinutes) };
        await cache.SetAsync(cacheKey, wordCollection, cacheOptions);
    }
}
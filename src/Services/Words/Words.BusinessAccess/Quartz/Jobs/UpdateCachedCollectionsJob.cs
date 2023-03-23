using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.BusinessAccess.Options;

namespace Words.BusinessAccess.Quartz.Jobs;

[DisallowConcurrentExecution]
public class UpdateCachedCollectionsJob : IJob
{
    private readonly IDistributedCache _cache;
    private readonly WordsDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IOptions<WordsRedisCacheOptions> _wordsRedisCacheOptions;

    public UpdateCachedCollectionsJob(IDistributedCache cache, 
        WordsDbContext dbContext, 
        IConfiguration configuration, 
        IOptions<WordsRedisCacheOptions> options)
    {
        _cache = cache;
        _dbContext = dbContext;
        _configuration = configuration;
        _wordsRedisCacheOptions = options;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        
        var wordCollections = await GetMostPopularWordCollections(_wordsRedisCacheOptions.Value.CachedCollectionsCount);
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(_wordsRedisCacheOptions.Value.SlidingExpirationTimeInMinutes) };
        foreach (var wordCollection in wordCollections)
        {
            var cacheKey = CacheHelper.GetCacheKeyForWordCollection(wordCollection.Id);
            
            await _cache.SetAsync(cacheKey, wordCollection, cacheOptions);
        }
    }
    
    private async Task<List<WordCollection>> GetMostPopularWordCollections(int count) 
        => await _dbContext.Collections
            .OrderByDescending(x => x.DailyViews)
            .Take(count)
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .ToListAsync();
}
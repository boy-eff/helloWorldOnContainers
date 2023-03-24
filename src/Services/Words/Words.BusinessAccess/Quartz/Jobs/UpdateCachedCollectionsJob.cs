using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;
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
    private readonly IOptions<WordsRedisCacheOptions> _wordsRedisCacheOptions;
    private readonly ILogger<UpdateCachedCollectionsJob> _logger;

    public UpdateCachedCollectionsJob(IDistributedCache cache, 
        WordsDbContext dbContext, 
        IOptions<WordsRedisCacheOptions> options, ILogger<UpdateCachedCollectionsJob> logger)
    {
        _cache = cache;
        _dbContext = dbContext;
        _wordsRedisCacheOptions = options;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{JobName} job started", context.JobDetail.Key.Name);
        var wordCollections = await GetMostPopularWordCollections(_wordsRedisCacheOptions.Value.CachedCollectionsCount);
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(_wordsRedisCacheOptions.Value.SlidingExpirationTimeInMinutes) };
        foreach (var wordCollection in wordCollections)
        {
            var cacheKey = CacheHelper.GetCacheKeyForWordCollection(wordCollection.Id);
            
            await _cache.SetAsync(cacheKey, wordCollection, cacheOptions);
        }
        _logger.LogInformation("{JobName} job finished", context.JobDetail.Key.Name);
    }
    
    private async Task<List<WordCollection>> GetMostPopularWordCollections(int count) 
        => await _dbContext.Collections
            .OrderByDescending(x => x.DailyViews)
            .Take(count)
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .ToListAsync();
}
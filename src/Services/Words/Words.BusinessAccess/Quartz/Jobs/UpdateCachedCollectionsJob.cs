using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using StackExchange.Redis;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.BusinessAccess.Extensions;

namespace Words.BusinessAccess.Quartz.Jobs;

[DisallowConcurrentExecution]
public class UpdateCachedCollectionsJob : IJob
{
    private readonly IDistributedCache _cache;
    private readonly WordsDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UpdateCachedCollectionsJob> _logger;

    public UpdateCachedCollectionsJob(IDistributedCache cache,
        WordsDbContext dbContext,
        IConfiguration configuration,
        ILogger<UpdateCachedCollectionsJob> logger)
    {
        _cache = cache;
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{JobName} job started", context.JobDetail.Key.Name);
        var cachedCollectionsCount = Convert.ToInt32(_configuration["Redis:CachedCollectionsCount"]);
        var minutesCount = Convert.ToInt32(_configuration["Redis:SlidingExpirationTimeInMinutes"]);
        var slidingExpirationTime = TimeSpan.FromMinutes(minutesCount);
        var wordCollections = await GetMostPopularWordCollections(cachedCollectionsCount);
        const string wordCollectionTypeName = nameof(WordCollection);
        foreach (var wordCollection in wordCollections)
        {
            await _cache.SetAsync(wordCollectionTypeName + wordCollection.Id, wordCollection);
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
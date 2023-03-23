using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.Quartz.Jobs;

[DisallowConcurrentExecution]
public class UpdateDailyWordCollectionJob : IJob
{
    private readonly WordsDbContext _dbContext;
    private readonly IDailyWordCollectionService _dailyWordCollectionService;
    private readonly ILogger<UpdateDailyWordCollectionJob> _logger;

    public UpdateDailyWordCollectionJob(WordsDbContext dbContext, 
        IDailyWordCollectionService dailyWordCollectionService, 
        ILogger<UpdateDailyWordCollectionJob> logger)
    {
        _dbContext = dbContext;
        _dailyWordCollectionService = dailyWordCollectionService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Start updating daily word collection");
        
        var dailyWordCollection = await _dbContext.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .FirstOrDefaultAsync(x 
                => x.DailyViews == _dbContext.Collections.Max(x => x.DailyViews));

        _dailyWordCollectionService.DailyWordCollection = dailyWordCollection;
        _logger.LogInformation("Daily word collection is {dailyWordCollection} with id {id}", dailyWordCollection.Name, dailyWordCollection.Id);
        await _dbContext.Collections.ForEachAsync(x => x.DailyViews = 0);
        _logger.LogInformation("End updating daily word collection");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Words.BusinessAccess.Contracts;
using Words.DataAccess;

namespace Words.BusinessAccess.Quartz.Jobs;

[DisallowConcurrentExecution]
public class UpdateViewsJob : IJob
{
    private readonly IViewsCounterService _viewsCounterService;
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<UpdateViewsJob> _logger;

    public UpdateViewsJob(IViewsCounterService viewsCounterService, WordsDbContext dbContext, ILogger<UpdateViewsJob> logger)
    {
        _viewsCounterService = viewsCounterService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var views = _viewsCounterService.GetViewsAndFlush();
        var wordCollections = _dbContext.Collections.Where(x => views.Keys.Contains(x.Id));
        _logger.LogInformation("Start updating views of word collections");
        foreach (var collection in wordCollections)
        {
            collection.DailyViews += views[collection.Id];
            collection.TotalViews += views[collection.Id];
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("End updating views of word collection");
    }
}
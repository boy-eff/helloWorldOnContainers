using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Serilog;
using Words.BusinessAccess.Contracts;

namespace Words.BusinessAccess.Services;

public class ViewsCounterService : IViewsCounterService
{
    private readonly ConcurrentDictionary<int, int> _viewsByCollectionId = new();
    private readonly ILogger<ViewsCounterService> _logger;

    public ViewsCounterService(ILogger<ViewsCounterService> logger)
    {
        _logger = logger;
    }

    public void IncrementViewsInCollection(int collectionId, int count = 1)
    {
        int currentViews;
        lock (_viewsByCollectionId)
        {
            _viewsByCollectionId.TryAdd(collectionId, 0);
            _viewsByCollectionId[collectionId] += count;
            currentViews = _viewsByCollectionId[collectionId];
        }
        _logger.LogInformation("Views of collection {CollectionId} were successfully incremented. Current views count: {ViewsCount}",
            collectionId, currentViews);
    }

    public IReadOnlyDictionary<int, int> GetViewsAndFlush()
    {
        ImmutableDictionary<int, int> immutableDictionary;
        lock (_viewsByCollectionId)
        {
            immutableDictionary = _viewsByCollectionId.ToImmutableDictionary();
            _viewsByCollectionId.Clear();
        }
        _logger.LogInformation("Views were successfully cleared");
        return immutableDictionary;
    }
}
using System.Collections.Concurrent;
using System.Collections.Immutable;
using Words.BusinessAccess.Contracts;

namespace Words.BusinessAccess.Services;

public class ViewsCounterService : IViewsCounterService
{
    private readonly ConcurrentDictionary<int, int> _viewsByCollectionId = new();

    public void IncrementViewsInCollection(int collectionId, int count = 1)
    {
        lock (_viewsByCollectionId)
        {
            _viewsByCollectionId.TryAdd(collectionId, 0);
            _viewsByCollectionId[collectionId] += count;
        }
    }

    public IReadOnlyDictionary<int, int> GetViewsAndFlush()
    {
        lock (_viewsByCollectionId)
        {
            var immutableDictionary = _viewsByCollectionId.ToImmutableDictionary();
            _viewsByCollectionId.Clear();
            return immutableDictionary;
        }
    }
}
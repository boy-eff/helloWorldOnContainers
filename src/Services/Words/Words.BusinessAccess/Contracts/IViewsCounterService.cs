namespace Words.BusinessAccess.Contracts;

public interface IViewsCounterService
{
    void IncrementViewsInCollection(int collectionId, int count = 1);
    IReadOnlyDictionary<int, int> GetViewsAndFlush();
}
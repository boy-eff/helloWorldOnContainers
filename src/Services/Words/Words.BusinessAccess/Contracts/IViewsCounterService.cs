namespace Words.BusinessAccess.Contracts;

public interface IViewsCounterService
{
    void Increment(int collectionId, int count = 1);
    IReadOnlyDictionary<int, int> GetViewsAndFlush();
}
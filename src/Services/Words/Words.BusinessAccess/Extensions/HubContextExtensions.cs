using Words.BusinessAccess.Models;
using Words.BusinessAccess.SignalR;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class HubContextExtensions
{
    public static void SetCollectionId(this IDictionary<object, object?> items, int collectionId) =>
        items[HubContextItems.CollectionId] = collectionId;
    public static int GetCollectionId(this IDictionary<object, object?> items) =>
        Convert.ToInt32(items[HubContextItems.CollectionId]);
    
    public static void SetTestEnumerator(this IDictionary<object, object?> items, IEnumerator<WordCollectionTest> enumerator) =>
        items[HubContextItems.TestEnumerator] = enumerator;

    public static IEnumerator<WordCollectionTest> GetTestEnumerator(this IDictionary<object, object?> items) =>
        items[HubContextItems.TestEnumerator] as IEnumerator<WordCollectionTest>;
    
    public static void SetTestPassInformation(this IDictionary<object, object?> items, WordCollectionTestPassInformation testPassInformation) =>
        items[HubContextItems.TestPassInformation] = testPassInformation;

    public static WordCollectionTestPassInformation GetTestPassInformation(this IDictionary<object, object?> items) =>
        items[HubContextItems.TestPassInformation] as WordCollectionTestPassInformation;
}
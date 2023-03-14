using Words.BusinessAccess.Models;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Contracts;

public interface IWordCollectionTestGenerator
{
    Task<IEnumerable<WordCollectionTest>> GenerateTestsFromCollection(int wordCollectionId, int answerOptionsCount);
}
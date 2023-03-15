using Words.BusinessAccess.Models;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Contracts;

public interface IWordCollectionTestGenerator
{
    Task<ICollection<WordCollectionTest>> GenerateTestsFromCollection(int wordCollectionId, int answerOptionsCount);
}
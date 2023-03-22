using Words.DataAccess.Models;

namespace Words.BusinessAccess.Models;

public class WordCollectionTest
{
    public Word Word { get; set; }
    public IList<AnswerOption> AnswerOptions { get; set; }
}
using Words.BusinessAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class WordCollectionTestExtensions
{
    public static string GetCorrectAnswerOptionValue(this WordCollectionTest test)
    {
        return test.AnswerOptions.FirstOrDefault(x => x.IsCorrect)?.Value;
    }
}
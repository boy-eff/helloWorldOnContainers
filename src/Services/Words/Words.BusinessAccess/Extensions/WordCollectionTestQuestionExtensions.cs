using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class WordCollectionTestQuestionExtensions
{
    public static bool IsUserAnswerCorrect(this WordCollectionTestQuestion question)
    {
        return question.CorrectAnswer == question.UserAnswer;
    }
}
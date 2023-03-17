using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class WordCollectionTestPassInformationExtensions
{
    public static void AddAnswerToTestPassInformation(this WordCollectionTestPassInformation testPassInformation,
        WordCollectionTestQuestion question)
    {
        testPassInformation.TotalQuestions++;
        if (question.IsCorrect)
        {
            testPassInformation.CorrectAnswersAmount++;
        }
        testPassInformation.Questions.Add(question);
    }
}
namespace Words.DataAccess.Models;

public class WordCollectionTestQuestion
{
    public WordCollectionTestQuestion(int wordCollectionTestPassInformationId, string correctAnswer, string userAnswer)
    {
        WordCollectionTestPassInformationId = wordCollectionTestPassInformationId;
        CorrectAnswer = correctAnswer;
        UserAnswer = userAnswer;
        IsCorrect = correctAnswer == userAnswer;
    }

    public int Id { get; set; }

    public int WordCollectionTestPassInformationId { get; set; }
    public WordCollectionTestPassInformation WordCollectionTestPassInformation { get; set; }
    
    public string CorrectAnswer { get; set; }
    public string UserAnswer { get; set; }
    public bool IsCorrect { get; set; }
}
namespace Shared.Messages;

public class WordCollectionTestPassedMessage
{
    public int WordCollectionId { get; set; }
    public int UserId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswersAmount { get; set; }
}
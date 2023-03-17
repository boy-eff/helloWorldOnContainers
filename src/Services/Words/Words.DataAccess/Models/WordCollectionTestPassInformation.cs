namespace Words.DataAccess.Models;

public class WordCollectionTestPassInformation
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int WordCollectionId { get; set; }
    public WordCollection WordCollection { get; set; }

    public ICollection<WordCollectionTestQuestion> Questions { get; set; } = new List<WordCollectionTestQuestion>();
    
    public int TotalQuestions { get; set; }
    public int CorrectAnswersAmount { get; set; }
}
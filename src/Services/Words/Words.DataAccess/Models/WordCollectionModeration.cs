namespace Words.DataAccess.Models;

public class WordCollectionModeration
{
    public int Id { get; set; }
    
    public int WordCollectionId { get; set; }
    public WordCollection WordCollection { get; set; }

    public int ModerationStatusId { get; set; }
    public ModerationStatus ModerationStatus { get; set; }
    public string Review { get; set; }
}
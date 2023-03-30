using Words.DataAccess.Enums;

namespace Words.DataAccess.Models;

public class WordCollectionModeration
{
    public int Id { get; set; }
    
    public int WordCollectionId { get; set; }
    public WordCollection WordCollection { get; set; }

    public ModerationStatusType ModerationStatusId { get; set; }
    public ModerationStatus ModerationStatus { get; set; }

    public int ModeratorId { get; set; }
    public User Moderator { get; set; }
    
    public string Review { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
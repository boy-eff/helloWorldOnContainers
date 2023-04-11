using Shared.Enums;
using Words.DataAccess.Enums;

namespace Words.DataAccess.Models;

public class WordCollection
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public int TotalViews { get; set; }
    public int DailyViews { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public int UserId { get; set; }
    public User User { get; set; }

    public ModerationStatusType ActualModerationStatus { get; set; } = ModerationStatusType.Pending;

    public IList<Word> Words { get; set; }
    public ICollection<WordCollectionRating> Ratings { get; set; }
    public ICollection<WordCollectionTestPassInformation> Tests { get; set; }
    public ICollection<WordCollectionModeration> Moderations { get; set; }
}
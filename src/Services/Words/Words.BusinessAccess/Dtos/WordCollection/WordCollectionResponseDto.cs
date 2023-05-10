using Shared.Enums;
using Words.DataAccess.Enums;

namespace Words.BusinessAccess.Dtos.WordCollection;

public class WordCollectionResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public string ImageUrl { get; set; }
    public ModerationStatusType ActualModerationStatus { get; set; }
    public List<WordDto> Words { get; set; }
}
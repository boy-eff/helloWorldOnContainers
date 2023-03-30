using Words.DataAccess.Enums;

namespace Words.BusinessAccess.Dtos;

public class WordCollectionModerationDto
{
    public int WordCollectionId { get; set; }
    public ModerationStatusType ModerationStatusId { get; set; }
    public string Review { get; set; }
}
using Words.DataAccess.Enums;

namespace Words.DataAccess.Models;

public class ModerationStatus
{
    public ModerationStatusType Id { get; set; }
    public string Name { get; set; }
}
using Words.DataAccess.Enums;
using Words.DataAccess.Models;

namespace Words.DataAccess.SeedData;

public static class SeedModerationStatuses
{
    public static ModerationStatus PendingStatus { get; set; } = new() { Id = (int)ModerationStatusType.Pending, Name = "Pending" };
    public static ModerationStatus AcceptedStatus { get; set; } = new() { Id = (int)ModerationStatusType.Accepted, Name = "Accepted" };
    public static ModerationStatus RejectedStatus { get; set; } = new() { Id = (int)ModerationStatusType.Rejected, Name = "Rejected" };
}
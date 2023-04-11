using Words.DataAccess.Enums;
using Words.DataAccess.Models;

namespace Words.DataAccess.SeedData;

public static class SeedModerationStatuses
{
    public static ModerationStatus PendingStatus { get; } = new() { Id = ModerationStatusType.Pending, Name = "Pending" };
    public static ModerationStatus AcceptedStatus { get; } = new() { Id = ModerationStatusType.Accepted, Name = "Accepted" };
    public static ModerationStatus RejectedStatus { get; } = new() { Id = ModerationStatusType.Rejected, Name = "Rejected" };
}
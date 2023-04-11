using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;
using Words.DataAccess.SeedData;

namespace Words.DataAccess.Configuration;

public class ModerationStatusConfiguration : IEntityTypeConfiguration<ModerationStatus>
{
    public void Configure(EntityTypeBuilder<ModerationStatus> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100);
        
        builder.HasData(
            SeedModerationStatuses.PendingStatus,
            SeedModerationStatuses.AcceptedStatus,
            SeedModerationStatuses.RejectedStatus);
    }
}
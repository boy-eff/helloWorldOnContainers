using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordCollectionModerationConfiguration : IEntityTypeConfiguration<WordCollectionModeration>
{
    public void Configure(EntityTypeBuilder<WordCollectionModeration> builder)
    {
        builder.Property(x => x.Review)
            .HasMaxLength(100);

        builder.HasOne(x => x.Moderator)
            .WithMany(x => x.Moderations)
            .HasForeignKey(x => x.ModeratorId);

        builder.HasOne(x => x.Moderator)
            .WithMany(x => x.Moderations)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
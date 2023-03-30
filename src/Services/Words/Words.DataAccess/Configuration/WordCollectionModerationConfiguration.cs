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
    }
}
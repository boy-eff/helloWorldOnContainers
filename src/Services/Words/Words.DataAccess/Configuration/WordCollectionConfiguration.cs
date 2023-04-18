using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Enums;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordCollectionConfiguration : IEntityTypeConfiguration<WordCollection>
{
    public void Configure(EntityTypeBuilder<WordCollection> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.EnglishLevel)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasPrecision(2);

        builder.Property(x => x.ActualModerationStatus)
            .HasDefaultValue(ModerationStatusType.Pending);
        
        builder.Property(x => x.ImageUrl)
            .HasMaxLength(100);

        builder.Property(x => x.ImagePublicId)
            .HasMaxLength(100);
    }
}
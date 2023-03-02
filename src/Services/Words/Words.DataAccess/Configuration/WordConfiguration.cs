using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordConfiguration : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(50);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordCollectionTestQuestionConfiguration : IEntityTypeConfiguration<WordCollectionTestQuestion>
{
    public void Configure(EntityTypeBuilder<WordCollectionTestQuestion> builder)
    {
        builder.Property(x => x.CorrectAnswer)
            .HasMaxLength(50);

        builder.Property(x => x.UserAnswer)
            .HasMaxLength(50);

        builder.Property(x => x.Word)
            .HasMaxLength(50);
    }
}
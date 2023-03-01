using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordCollectionRatingConfiguration : IEntityTypeConfiguration<WordCollectionRating>
{
    public void Configure(EntityTypeBuilder<WordCollectionRating> builder)
    {
        builder
            .HasOne<User>(x => x.User)
            .WithMany(x => x.CollectionRatings)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasPrecision(2);
    }
}
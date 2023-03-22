using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class WordCollectionTestPassInformationConfiguration : IEntityTypeConfiguration<WordCollectionTestPassInformation>
{
    public void Configure(EntityTypeBuilder<WordCollectionTestPassInformation> builder)
    {
        builder.HasOne<User>(x => x.User)
            .WithMany(x => x.TestsPassInformation)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
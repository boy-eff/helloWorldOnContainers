using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class UserWordConfiguration : IEntityTypeConfiguration<UserWord>
{
    public void Configure(EntityTypeBuilder<UserWord> builder)
    {
        builder.HasKey(x => new { x.UserId, x.WordId });

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.DictionaryWords)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Word)
            .WithMany(x => x.UserDictionaries)
            .HasForeignKey(x => x.WordId);
    }
}
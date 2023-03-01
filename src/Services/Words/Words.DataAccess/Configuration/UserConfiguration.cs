using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Words.DataAccess.Models;

namespace Words.DataAccess.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.UserName)
            .HasMaxLength(50);
        
        builder.Property(x => x.EnglishLevel)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt).HasPrecision(2);
    }
}
using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Achievements.Domain.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.UserName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(x => x.UsersAchievements)
            .WithOne(x => x.User);
    }
}
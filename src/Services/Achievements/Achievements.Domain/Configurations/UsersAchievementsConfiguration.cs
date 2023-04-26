using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Achievements.Domain.Configurations;

public class UsersAchievementsConfiguration : IEntityTypeConfiguration<UsersAchievements>
{
    public void Configure(EntityTypeBuilder<UsersAchievements> builder)
    {
        builder.HasKey(x => new { x.AchievementId, x.UserId });

        builder.HasOne(x => x.NextLevel)
            .WithMany(x => x.UsersAchievements)
            .HasForeignKey(x => new { x.AchievementId, x.NextLevelId })
            .OnDelete(DeleteBehavior.SetNull);
    }
}
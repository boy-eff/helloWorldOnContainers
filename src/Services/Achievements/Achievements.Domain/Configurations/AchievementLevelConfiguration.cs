using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Achievements.Domain.Configurations;

public class AchievementLevelConfiguration : IEntityTypeConfiguration<AchievementLevel>
{
    public void Configure(EntityTypeBuilder<AchievementLevel> builder)
    {
        builder.HasKey(x => new { x.AchievementId, x.Level });
    }
}
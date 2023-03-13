using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IAchievementLevelRepository
{
    Task<AchievementLevel> GetById(int achievementId, int levelId);
}
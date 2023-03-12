using Achievements.Domain.Contracts;
using Achievements.Domain.Models;

namespace Achievements.Domain.Repositories;

public class AchievementLevelRepository : IAchievementLevelRepository
{
    private readonly AchievementsDbContext _dbContext;

    public AchievementLevelRepository(AchievementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AchievementLevel> GetById(int achievementId, int levelId)
    {
        return await _dbContext.AchievementLevels.FindAsync(achievementId, levelId);
    }
}
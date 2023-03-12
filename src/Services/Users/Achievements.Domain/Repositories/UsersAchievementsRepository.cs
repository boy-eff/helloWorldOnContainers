using Achievements.Domain.Contracts;
using Achievements.Domain.Models;

namespace Achievements.Domain.Repositories;

public class UsersAchievementsRepository : IUsersAchievementsRepository
{
    private readonly AchievementsDbContext _dbContext;

    public UsersAchievementsRepository(AchievementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UsersAchievements> GetAsync(int userId, int achievementId)
    {
        return await _dbContext.UsersAchievements.FindAsync(userId, achievementId);
    }

    public async Task AddAsync(UsersAchievements usersAchievements)
    {
        await _dbContext.UsersAchievements.AddAsync(usersAchievements);
    }
}
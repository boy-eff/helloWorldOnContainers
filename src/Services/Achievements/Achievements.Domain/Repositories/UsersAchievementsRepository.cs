using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Domain.Repositories;

public class UsersAchievementsRepository : IUsersAchievementsRepository
{
    private readonly AchievementsDbContext _dbContext;

    public UsersAchievementsRepository(AchievementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UsersAchievements?> GetAsync(int achievementId, int userId)
    {
        return await _dbContext.UsersAchievements.FindAsync(achievementId, userId);
    }

    public async Task<UsersAchievements?> GetWithUserAsync(int userId, int achievementId)
    {
        return await _dbContext.UsersAchievements
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.AchievementId == achievementId);
    }

    public async Task AddAsync(UsersAchievements usersAchievements)
    {
        await _dbContext.UsersAchievements.AddAsync(usersAchievements);
    }

    public void Update(UsersAchievements usersAchievement)
    {
        _dbContext.UsersAchievements.Update(usersAchievement);
    }
}
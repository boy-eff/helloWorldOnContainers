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

    public async Task<IEnumerable<UsersAchievements>> GetByUserAsync(int userId)
    {
        var usersAchievements = await _dbContext.UsersAchievements
            .Where(x => x.UserId == userId)
            .Include(x => x.NextLevel)
            .Include(x => x.Achievement)
            .AsNoTracking()
            .ToListAsync();

        var achievements = SeedData.Achievements;
        foreach (var achievement in achievements)
        {
            if (usersAchievements.All(x => x.Achievement.Id != achievement.Id))
            {
                var userAchievement = new UsersAchievements()
                {
                    Achievement = achievement,
                    NextLevel = achievement.Levels.First(),
                    PointsAchieved = 0,
                    UserId = userId
                };
                usersAchievements.Add(userAchievement);
            }
        }

        return usersAchievements;
    }

    public async Task AddAsync(UsersAchievements usersAchievements)
    {
        await _dbContext.UsersAchievements.AddAsync(usersAchievements);
        _dbContext.Entry(usersAchievements.NextLevel).State = EntityState.Unchanged;
    }

    public void Update(UsersAchievements usersAchievement)
    {
        _dbContext.UsersAchievements.Update(usersAchievement);
        _dbContext.Entry(usersAchievement.NextLevel).State = EntityState.Unchanged;
    }
}
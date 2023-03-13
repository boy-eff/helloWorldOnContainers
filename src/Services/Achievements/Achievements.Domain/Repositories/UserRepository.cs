using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Domain.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AchievementsDbContext _dbContext;

    public UserRepository(AchievementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetUserWithAchievementById(int userId, int achievementId)
    {
        return await _dbContext.Users
            .Include(u => u.UsersAchievements.Where(ua => ua.AchievementId == achievementId))
            .ThenInclude(ua => ua.Achievement)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public Task<IEnumerable<User>> GetUserWithAchievementsById()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _dbContext.Update(user);
    }
}
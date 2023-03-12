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

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _dbContext.Update(user);
    }
}
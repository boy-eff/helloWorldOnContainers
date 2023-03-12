using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IUsersAchievementsRepository
{
    Task<UsersAchievements> GetAsync(int userId, int achievementId);
    Task AddAsync(UsersAchievements usersAchievements);
}
using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IUsersAchievementsRepository
{
    Task<UsersAchievements?> GetAsync(int achievementId, int userId);
    Task<UsersAchievements?> GetWithUserAsync(int achievementId, int userId);
    Task AddAsync(UsersAchievements usersAchievements);
    void Update(UsersAchievements usersAchievement);
}
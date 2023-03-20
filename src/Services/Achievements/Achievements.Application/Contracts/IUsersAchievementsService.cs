using Achievements.Domain.Models;

namespace Achievements.Application.Contracts;

public interface IUsersAchievementsService
{
    Task UpdateUsersAchievementsLevelAsync(User user, int achievementId, AchievementLevel achievementLevel);
}
using Achievements.Application.Dtos;
using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Application.Contracts;

public interface IUsersAchievementsService
{
    Task<UsersAchievements?> UpsertUsersAchievementsLevelAsync(User user, int achievementId);
    Task<IEnumerable<UsersAchievementsDto>> GetUserAchievementsByIdAsync(int userId);
}
using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Application.Contracts;

public interface IUserAchievementIncrementor
{
    public AchievementType AchievementType { get; set; }
    void IncrementAchievementPoints(User user, int points = 1);
}
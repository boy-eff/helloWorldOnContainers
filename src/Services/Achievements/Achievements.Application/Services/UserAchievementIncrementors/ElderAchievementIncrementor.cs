using Achievements.Application.Contracts;
using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Application.Services.UserAchievementIncrementors;

public class ElderAchievementIncrementor : IUserAchievementIncrementor
{
    public AchievementType AchievementType { get; set; } = AchievementType.Elder;

    public void IncrementAchievementPoints(User user, int points = 1)
    {
        user.YearsInAppAmount += points;
    }
}
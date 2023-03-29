using Achievements.Application.Contracts;
using Achievements.Domain.Models;

namespace Achievements.Application.Services.UserAchievementIncrementors;

public class ElderAchievementIncrementor : IUserAchievementIncrementor
{
    public void IncrementAchievementPoints(User user, int points = 1)
    {
        user.YearsInAppAmount += points;
    }
}
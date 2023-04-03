using Achievements.Application.Contracts;
using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Application.Services.UserAchievementIncrementors;

public class QuizConquerorAchievementIncrementor : IUserAchievementIncrementor
{
    public AchievementType AchievementType { get; set; } = AchievementType.QuizConqueror;

    public void IncrementAchievementPoints(User user, int points = 1)
    {
        user.CollectionTestsPassedAmount += points;
    }
}
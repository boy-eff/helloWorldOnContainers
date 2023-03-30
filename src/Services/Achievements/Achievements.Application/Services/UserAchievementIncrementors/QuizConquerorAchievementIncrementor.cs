using Achievements.Application.Contracts;
using Achievements.Domain.Models;

namespace Achievements.Application.Services.UserAchievementIncrementors;

public class QuizConquerorAchievementIncrementor : IUserAchievementIncrementor
{
    public void IncrementAchievementPoints(User user, int points = 1)
    {
        user.CollectionTestsPassedAmount += points;
    }
}
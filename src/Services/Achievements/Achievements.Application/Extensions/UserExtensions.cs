using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Application.Extensions;

public static class UserExtensions
{
    public static void AddBalanceAndExperience(this User user, int balance, int experience)
    {
        user.Balance += balance;
        user.Experience += experience;
    }

    public static int GetAchievementPoints(this User user, int achievementId)
    {
        var achievementType = (AchievementType)achievementId;
        switch (achievementType)
        {
            case AchievementType.Collector:
                return user.WordsInDictionaryAmount;
                break;
            case AchievementType.Creator:
                return user.CollectionsCreatedAmount;
                break;
            case AchievementType.QuizConqueror:
                return user.CollectionTestsPassedAmount;
                break;
            case AchievementType.Elder:
                return user.YearsInAppAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(achievementType), achievementType, null);
        }
    }
}
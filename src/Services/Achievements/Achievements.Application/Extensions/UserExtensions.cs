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
        return achievementType switch
        {
            AchievementType.Collector => user.WordsInDictionaryAmount,
            AchievementType.Creator => user.CollectionsCreatedAmount,
            AchievementType.QuizConqueror => user.CollectionTestsPassedAmount,
            AchievementType.Elder => user.YearsInAppAmount,
            _ => throw new ArgumentOutOfRangeException(nameof(achievementType), achievementType, null)
        };
    }
}
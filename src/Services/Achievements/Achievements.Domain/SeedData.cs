using Achievements.Domain.Enums;
using Achievements.Domain.Models;

namespace Achievements.Domain;

public static class SeedData
{
    public static Achievement CollectorAchievement { get; } = new()
    {
        Id = (int)AchievementType.Collector,
        Name = "Collector",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = (int)AchievementType.Collector,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = (int)AchievementType.Collector,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = (int)AchievementType.Collector,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = (int)AchievementType.Collector,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = (int)AchievementType.Collector,
                PointsToAchieve = 2000
            },
        }
    };

    public static Achievement CreatorAchievement { get; } = new()
    {
        Id = (int)AchievementType.Creator,
        Name = "Creator",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = (int)AchievementType.Creator,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = (int)AchievementType.Creator,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = (int)AchievementType.Creator,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = (int)AchievementType.Creator,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = (int)AchievementType.Creator,
                PointsToAchieve = 2000
            },
        }
    };
    
    public static Achievement QuizConquerorAchievement { get; } = new()
    {
        Id = (int)AchievementType.QuizConqueror,
        Name = "Quiz Conqueror",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = (int)AchievementType.QuizConqueror,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = (int)AchievementType.QuizConqueror,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = (int)AchievementType.QuizConqueror,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = (int)AchievementType.QuizConqueror,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = (int)AchievementType.QuizConqueror,
                PointsToAchieve = 2000
            },
        }
    };
    
    public static Achievement ElderAchievement { get; set; } = new()
    {
        Id = (int)AchievementType.Elder,
        Name = "Elder",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 1,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 2,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 2
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 3,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 3
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 4,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 4
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 5,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 5
            },
            new AchievementLevel()
            {
                Experience = 100000,
                Reward = 50000,
                Level = 6,
                AchievementId = (int)AchievementType.Elder,
                PointsToAchieve = 10
            },
        }
    };
    public static IEnumerable<Achievement> Achievements { get; } = new List<Achievement>()
    {
        CollectorAchievement,
        ElderAchievement,
        QuizConquerorAchievement,
        CreatorAchievement
    };
}
using Achievements.Domain.Models;

namespace Achievements.Domain;

public static class SeedData
{
    public static Achievement CollectorAchievement { get; } = new()
    {
        Id = 1,
        Name = "Collector",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = 1,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = 1,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = 1,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = 1,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = 1,
                PointsToAchieve = 2000
            },
        }
    };

    public static Achievement CreatorAchievement { get; } = new()
    {
        Id = 1,
        Name = "Creator",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = 1,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = 1,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = 1,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = 1,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = 1,
                PointsToAchieve = 2000
            },
        }
    };
    
    public static Achievement QuizConquerorAchievement { get; } = new()
    {
        Id = 1,
        Name = "Quiz Conqueror",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 100,
                Reward = 50,
                Level = 1,
                AchievementId = 1,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 300,
                Reward = 100,
                Level = 2,
                AchievementId = 1,
                PointsToAchieve = 10
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 200,
                Level = 3,
                AchievementId = 1,
                PointsToAchieve = 100
            },
            new AchievementLevel()
            {
                Experience = 3000,
                Reward = 500,
                Level = 4,
                AchievementId = 1,
                PointsToAchieve = 500
            },
            new AchievementLevel()
            {
                Experience = 10000,
                Reward = 3000,
                Level = 5,
                AchievementId = 1,
                PointsToAchieve = 2000
            },
        }
    };
    
    public static Achievement ElderAchievement { get; set; } = new()
    {
        Id = 2,
        Name = "Elder",
        Levels = new List<AchievementLevel>()
        {
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 1,
                AchievementId = 2,
                PointsToAchieve = 1
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 2,
                AchievementId = 2,
                PointsToAchieve = 2
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 3,
                AchievementId = 2,
                PointsToAchieve = 3
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 4,
                AchievementId = 2,
                PointsToAchieve = 4
            },
            new AchievementLevel()
            {
                Experience = 1000,
                Reward = 500,
                Level = 5,
                AchievementId = 2,
                PointsToAchieve = 5
            },
            new AchievementLevel()
            {
                Experience = 100000,
                Reward = 50000,
                Level = 6,
                AchievementId = 2,
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
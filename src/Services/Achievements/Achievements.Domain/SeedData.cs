﻿using Achievements.Domain.Models;

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
    public static IEnumerable<Achievement> Achievements { get; } = new List<Achievement>()
    {
        CollectorAchievement
    };
}
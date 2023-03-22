namespace Achievements.Domain.Models;

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<AchievementLevel> Levels { get; set; }
    public ICollection<UsersAchievements> UsersAchievements { get; set; }
}
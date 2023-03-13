namespace Achievements.Domain.Models;

public class UsersAchievements
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int AchievementId { get; set; }
    public Achievement Achievement { get; set; }

    public int CurrentLevel { get; set; }
    public DateTimeOffset AchieveDate { get; set; }
}
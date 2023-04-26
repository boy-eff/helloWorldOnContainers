namespace Achievements.Domain.Models;

public class UsersAchievements
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int AchievementId { get; set; }
    public Achievement Achievement { get; set; }
    public int PointsAchieved { get; set; }
    public int NextLevelId { get; set; }
    public AchievementLevel NextLevel { get; set; }
    public DateTime AchieveDate { get; set; }
}
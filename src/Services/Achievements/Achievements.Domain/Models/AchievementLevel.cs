namespace Achievements.Domain.Models;

public class AchievementLevel
{
    public int AchievementId { get; set; }
    public int Level { get; set; }
    
    public int PointsToAchieve { get; set; }
    public int Reward { get; set; }
    public int Experience { get; set; }
    
    
    public Achievement Achievement { get; set; }
    public ICollection<UsersAchievements> UsersAchievements { get; set; }
}
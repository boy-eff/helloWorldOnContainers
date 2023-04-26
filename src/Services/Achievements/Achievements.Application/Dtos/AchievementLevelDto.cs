namespace Achievements.Application.Dtos;

public class AchievementLevelDto
{
    public int AchievementId { get; set; }
    public int Level { get; set; }
    
    public int PointsToAchieve { get; set; }
    public int Reward { get; set; }
    public int Experience { get; set; }
}
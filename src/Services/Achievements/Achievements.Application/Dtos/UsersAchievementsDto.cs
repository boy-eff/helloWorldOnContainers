using Achievements.Domain.Models;

namespace Achievements.Application.Dtos;

public class UsersAchievementsDto
{
    public int UserId { get; set; }
    public AchievementDto Achievement { get; set; }
    public int PointsAchieved { get; set; }
    public AchievementLevelDto NextLevel { get; set; }
    public DateTime AchieveDate { get; set; }
}
namespace Achievements.Application.Dtos;

public class UsersAchievementsDto
{
    public int UserId { get; set; }
    public int AchievementId { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime AchieveDate { get; set; }
}
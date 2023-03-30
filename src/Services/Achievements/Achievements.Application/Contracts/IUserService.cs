namespace Achievements.Application.Contracts;

public interface IUserService
{
    Task UpdateAchievementPointsAsync(int userId, IUserAchievementIncrementor achievementIncrementor);
}
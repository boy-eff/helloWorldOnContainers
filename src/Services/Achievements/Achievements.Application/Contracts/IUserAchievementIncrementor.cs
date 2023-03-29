using Achievements.Domain.Models;

namespace Achievements.Application.Contracts;

public interface IUserAchievementIncrementor
{
    public void IncrementAchievementPoints(User user, int points = 1);
}
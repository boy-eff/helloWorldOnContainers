using Achievements.Domain.Models;

namespace Achievements.Application.Contracts;

public interface IUserAchievementIncrementor
{ 
    void IncrementAchievementPoints(User user, int points = 1);
}
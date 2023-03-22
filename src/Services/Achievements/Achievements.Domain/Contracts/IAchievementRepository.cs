using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAchievementByUserId();
}
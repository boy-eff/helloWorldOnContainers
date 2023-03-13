using System.Collections;
using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserWithAchievementById(int userId, int achievementId);
    Task AddAsync(User user);
    void Update(User user);
}
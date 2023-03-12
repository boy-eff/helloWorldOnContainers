using Achievements.Domain.Models;

namespace Achievements.Domain.Contracts;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetUsersAsync();
    Task AddAsync(User user);
    void Update(User user);
}
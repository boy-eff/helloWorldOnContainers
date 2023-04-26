namespace Achievements.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int Experience { get; set; }
    public int Balance { get; set; }
    public int CurrentStreak { get; set; }

    public ICollection<UsersAchievements> UsersAchievements { get; set; }
}
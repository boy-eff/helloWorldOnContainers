using Achievements.Application.Contracts;
using Achievements.Application.Extensions;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;

namespace Achievements.Application.Services;

public class UsersAchievementsService : IUsersAchievementsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersAchievementsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateUsersAchievementsLevelAsync(User user, int achievementId, AchievementLevel achievementLevel)
    {
        var usersAchievements = await _unitOfWork.UsersAchievementsRepository.GetAsync(achievementId, user.Id);

        if (usersAchievements is null)
        {
            await AddUsersAchievementsAsync(user.Id, achievementId, achievementLevel);
        }
        else
        {
            UpdateUsersAchievementsLevel(usersAchievements, achievementLevel);
        }
        
        user.AddBalanceAndExperience(achievementLevel.Reward, achievementLevel.Experience);
        await _unitOfWork.SaveChangesAsync();
    }

    private void UpdateUsersAchievementsLevel(UsersAchievements usersAchievements, AchievementLevel achievementLevel)
    {
        usersAchievements.CurrentLevel = achievementLevel.Level;
        _unitOfWork.UsersAchievementsRepository.Update(usersAchievements);
    }

    private async Task AddUsersAchievementsAsync(int userId, int achievementId, AchievementLevel achievementLevel)
    {
        var usersAchievements = new UsersAchievements()
        {
            AchievementId = achievementId,
            UserId = userId,
            CurrentLevel = achievementLevel.Level,
            AchieveDate = DateTime.Now
        };
        await _unitOfWork.UsersAchievementsRepository.AddAsync(usersAchievements);
    }
}
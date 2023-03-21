using Achievements.Application.Contracts;
using Achievements.Application.Dtos;
using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

namespace Achievements.Application.Services;

public class UsersAchievementsService : IUsersAchievementsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersAchievementsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UsersAchievements?> UpsertUsersAchievementsLevelAsync(User user, int achievementId)
    {
        var achievementLevel = SeedData.CollectorAchievement.Levels
            .FirstOrDefault(x => x.PointsToAchieve == user.GetAchievementPoints(achievementId));

        if (achievementLevel is null)
        {
            return null;
        }
        
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
        return usersAchievements;
    }

    public async Task<IEnumerable<UsersAchievementsDto>> GetUserAchievementsByIdAsync(int userId)
    {
        var usersAchievements = await _unitOfWork.UsersAchievementsRepository.GetByUserAsync(userId);
        return usersAchievements.Adapt<IEnumerable<UsersAchievementsDto>>();
    }
    
    private void UpdateUsersAchievementsLevel(UsersAchievements usersAchievements, AchievementLevel achievementLevel)
    {
        usersAchievements.CurrentLevel = achievementLevel.Level;
        usersAchievements.AchieveDate = DateTime.Now;
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
using Achievements.Application.Contracts;
using Achievements.Application.Dtos;
using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

namespace Achievements.Application.Services;

public class UsersAchievementsService : IUsersAchievementsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UsersAchievementsService> _logger;

    public UsersAchievementsService(IUnitOfWork unitOfWork, ILogger<UsersAchievementsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UsersAchievements?> UpsertUsersAchievementsLevelAsync(User user, int achievementId)
    {
        var achievementLevel = SeedData.Achievements.FirstOrDefault(x => x.Id == achievementId)
            ?.Levels
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
        
        _logger.LogInformation("User {UserId} gained {AchievementLevel} level of achievement {AchievementId}",
            user.Id, achievementLevel.Level, achievementId);
        
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
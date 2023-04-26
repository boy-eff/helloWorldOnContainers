using Achievements.Application.Contracts;
using Achievements.Application.Dtos;
using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using Microsoft.Extensions.Logging;

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

    public async Task<UsersAchievements?> UpsertUsersAchievementsLevelAsync(int userId, int achievementId)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var usersAchievementsExist = true;
            var usersAchievements = await _unitOfWork.UsersAchievementsRepository.GetAsync(achievementId, userId);

            if (usersAchievements is null)
            {
                usersAchievementsExist = false;
                usersAchievements = CreateUsersAchievements(userId, achievementId);
            }

            usersAchievements.PointsAchieved++;
            await _unitOfWork.SaveChangesAsync();

            var achievementLevel = SeedData.Achievements.FirstOrDefault(x => x.Id == achievementId)
                ?.Levels
                .FirstOrDefault(x => x.PointsToAchieve == usersAchievements.PointsAchieved);

            if (achievementLevel is null)
            {
                await _unitOfWork.CommitAsync();
                return null;
            }

            UpdateUsersAchievementsLevel(usersAchievements, achievementLevel);
            
            if (usersAchievementsExist)
            {
                _unitOfWork.UsersAchievementsRepository.Update(usersAchievements);
            }
            else
            {
                await _unitOfWork.UsersAchievementsRepository.AddAsync(usersAchievements);
            }

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);   

            user.AddBalanceAndExperience(achievementLevel.Reward, achievementLevel.Experience);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("User {UserId} gained {AchievementLevel} level of achievement {AchievementId}",
                user.Id, achievementLevel.Level, achievementId);
        
            return usersAchievements;
        }
        catch
        {
            await transaction.RollbackAsync();
            _logger.LogInformation("Error while updating achievements for user {UserId}", userId);
            throw;
        }
        
    }

    public async Task<IEnumerable<UsersAchievementsDto>> GetUserAchievementsByIdAsync(int userId)
    {
        var usersAchievements = await _unitOfWork.UsersAchievementsRepository.GetByUserAsync(userId);
        return usersAchievements.Adapt<IEnumerable<UsersAchievementsDto>>();
    }

    private void UpdateUsersAchievementsLevel(UsersAchievements usersAchievements, AchievementLevel achievementLevel)
    {
        var nextLevel = SeedData.Achievements.FirstOrDefault(x => x.Id == achievementLevel.AchievementId)
            .Levels.FirstOrDefault(x => x.Level == achievementLevel.Level + 1);
        usersAchievements.NextLevel = nextLevel;
        usersAchievements.AchieveDate = DateTime.Now;
    }

    private UsersAchievements CreateUsersAchievements(int userId, int achievementId)
    {
        var usersAchievements = new UsersAchievements()
        {
            AchievementId = achievementId,
            UserId = userId,
            AchieveDate = DateTime.Now
        };
        return usersAchievements;
    }
}
using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;

namespace Achievements.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public UserService(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task UpdateAchievementPointsAsync(int userId, IUserAchievementIncrementor achievementIncrementor)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            achievementIncrementor.IncrementAchievementPoints(user);
            await _unitOfWork.SaveChangesAsync();
        
            await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
            await _unitOfWork.CommitAsync();
        }
        catch(Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
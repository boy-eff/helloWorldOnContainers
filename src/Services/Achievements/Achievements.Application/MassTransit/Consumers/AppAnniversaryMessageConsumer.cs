using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class AppAnniversaryMessageConsumer : IConsumer<AppAnniversaryMessage>
{
    private readonly IUsersAchievementsService _usersAchievementsService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;

    public AppAnniversaryMessageConsumer(IUsersAchievementsService usersAchievementsService, IUnitOfWork unitOfWork, ILogger<AppAnniversaryMessageConsumer> logger)
    {
        _usersAchievementsService = usersAchievementsService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AppAnniversaryMessage> context)
    {
        _logger.LogInformation("User {UserId} is active in app for {YearsCount} years", context.Message.UserId, context.Message.Years);
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.UserId);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.YearsInAppAmount = context.Message.Years;
            await _unitOfWork.SaveChangesAsync();
        
            var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
            await _unitOfWork.CommitAsync();
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", user.Id);
    }
}
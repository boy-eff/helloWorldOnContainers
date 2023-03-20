using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
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
        var achievementLevel = SeedData.ElderAchievement.Levels.FirstOrDefault(x => x.PointsToAchieve == context.Message.Years);
        
        if (achievementLevel is null)
        {
            return;
        }

        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.UserId);

        if (user is null)
        {
            _logger.LogError("User with id {id} is not found", context.Message.UserId);
            throw new Exception();
        }
        
        var achievementId = SeedData.ElderAchievement.Id;
        await _usersAchievementsService.UpdateUsersAchievementsLevelAsync(user, achievementId,
            achievementLevel);
    }
}
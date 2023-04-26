using Achievements.Application.Contracts;
using Achievements.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class AppAnniversaryMessageConsumer : IConsumer<AppAnniversaryMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public AppAnniversaryMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUsersAchievementsService usersAchievementsService)
    {
        _logger = logger;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<AppAnniversaryMessage> context)
    {
        _logger.LogInformation("User {UserId} is active in app for {YearsCount} years", context.Message.UserId, context.Message.Years);
        await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(context.Message.UserId,
            SeedData.ElderAchievement.Id);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.UserId);
    }
}
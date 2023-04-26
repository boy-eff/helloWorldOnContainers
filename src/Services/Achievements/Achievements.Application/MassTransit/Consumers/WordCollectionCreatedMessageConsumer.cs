using Achievements.Application.Contracts;
using Achievements.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionCreatedMessageConsumer : IConsumer<WordCollectionCreatedMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordCollectionCreatedMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUsersAchievementsService usersAchievementsService)
    {
        _logger = logger;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordCollectionCreatedMessage> context)
    {
        await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(context.Message.CreatorId,
            SeedData.CreatorAchievement.Id);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.CreatorId);
    }
}
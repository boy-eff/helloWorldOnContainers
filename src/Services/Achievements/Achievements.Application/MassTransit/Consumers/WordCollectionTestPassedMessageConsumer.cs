using Achievements.Application.Contracts;
using Achievements.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionTestPassedMessageConsumer : IConsumer<WordCollectionTestPassedMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordCollectionTestPassedMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUsersAchievementsService usersAchievementsService)
    {
        _logger = logger;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordCollectionTestPassedMessage> context)
    {
        await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(context.Message.UserId,
            SeedData.QuizConquerorAchievement.Id);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.UserId);
    }
}
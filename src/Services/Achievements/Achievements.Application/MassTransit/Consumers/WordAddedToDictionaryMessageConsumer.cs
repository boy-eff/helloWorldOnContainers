using Achievements.Application.Contracts;
using Achievements.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordAddedToDictionaryMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUsersAchievementsService usersAchievementsService)
    {
        _logger = logger;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(context.Message.DictionaryOwnerId,
            SeedData.CollectorAchievement.Id);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.DictionaryOwnerId);
    }
}
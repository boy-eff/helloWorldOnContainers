using Achievements.Application.Contracts;
using Achievements.Application.Services.UserAchievementIncrementors;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUserService _userService;

    public WordAddedToDictionaryMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        var incrementor = new CollectorAchievementIncrementor();
        await _userService.UpdateAchievementPointsAsync(context.Message.DictionaryOwnerId, incrementor);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.DictionaryOwnerId);
    }
}
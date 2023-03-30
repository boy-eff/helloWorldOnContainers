﻿using Achievements.Application.Contracts;
using Achievements.Application.Services.UserAchievementIncrementors;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionTestPassedMessageConsumer : IConsumer<WordCollectionTestPassedMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUserService _userService;

    public WordCollectionTestPassedMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<WordCollectionTestPassedMessage> context)
    {
        var incrementor = new QuizConquerorAchievementIncrementor();
        await _userService.UpdateAchievementPointsAsync(context.Message.UserId, incrementor);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.UserId);
    }
}
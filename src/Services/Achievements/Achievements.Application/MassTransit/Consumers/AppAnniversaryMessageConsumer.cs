using Achievements.Application.Contracts;
using Achievements.Application.Services.UserAchievementIncrementors;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class AppAnniversaryMessageConsumer : IConsumer<AppAnniversaryMessage>
{
    private readonly ILogger<AppAnniversaryMessageConsumer> _logger;
    private readonly IUserService _userService;

    public AppAnniversaryMessageConsumer(ILogger<AppAnniversaryMessageConsumer> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<AppAnniversaryMessage> context)
    {
        _logger.LogInformation("User {UserId} is active in app for {YearsCount} years", context.Message.UserId, context.Message.Years);
        var incrementor = new ElderAchievementIncrementor();
        await _userService.UpdateAchievementPointsAsync(context.Message.UserId, incrementor);
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", context.Message.UserId);
    }
}
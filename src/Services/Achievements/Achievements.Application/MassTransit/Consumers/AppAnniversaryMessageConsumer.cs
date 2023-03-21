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
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.UserId);
        user.YearsInAppAmount = context.Message.Years;

        if (user is null)
        {
            _logger.LogError("User with id {id} is not found", context.Message.UserId);
            throw new Exception();
        }
        
        var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);

        if (result is null)
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
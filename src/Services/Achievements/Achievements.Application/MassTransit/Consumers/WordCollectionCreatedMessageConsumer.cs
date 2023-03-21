using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionCreatedMessageConsumer : IConsumer<WordCollectionCreatedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordCollectionCreatedMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordCollectionCreatedMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.CreatorId);
        user.CollectionsCreatedAmount++;

        var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.CreatorAchievement.Id);

        if (result is null)
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
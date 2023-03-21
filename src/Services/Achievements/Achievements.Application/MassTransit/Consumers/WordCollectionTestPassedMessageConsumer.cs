using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionTestPassedMessageConsumer : IConsumer<WordCollectionTestPassedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordCollectionTestPassedMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordCollectionTestPassedMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.UserId);
        user.CollectionTestsPassedAmount++;
        
        var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.QuizConquerorAchievement.Id);

        if (result is null)
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
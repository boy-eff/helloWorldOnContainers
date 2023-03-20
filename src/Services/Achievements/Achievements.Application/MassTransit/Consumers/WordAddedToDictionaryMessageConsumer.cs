using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;
    private readonly IDistributedCache _distributedCache;

    public WordAddedToDictionaryMessageConsumer(IUnitOfWork unitOfWork, IDistributedCache distributedCache, IUsersAchievementsService usersAchievementsService)
    {
        _unitOfWork = unitOfWork;
        _distributedCache = distributedCache;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        var collectorAchievement = SeedData.CollectorAchievement;
        var achievementId = collectorAchievement.Id;
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.DictionaryOwnerId);
        user.WordsInDictionaryAmount++;
        
        var achievementLevel = SeedData.CollectorAchievement.Levels
            .FirstOrDefault(x => x.PointsToAchieve == user.WordsInDictionaryAmount);

        if (achievementLevel is null)
        {
            await _unitOfWork.SaveChangesAsync();
            return;
        }
        
        await _usersAchievementsService.UpdateUsersAchievementsLevelAsync(user, achievementId, achievementLevel);
    }
}
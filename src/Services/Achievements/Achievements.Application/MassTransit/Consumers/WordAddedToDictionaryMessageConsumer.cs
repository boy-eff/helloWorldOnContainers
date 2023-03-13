using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _distributedCache;

    public WordAddedToDictionaryMessageConsumer(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
    {
        _unitOfWork = unitOfWork;
        _distributedCache = distributedCache;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        var collectorAchievement = SeedData.CollectorAchievement;
        var achievementId = collectorAchievement.Id;
        var user = await _unitOfWork.UserRepository.GetUserWithAchievementById(context.Message.DictionaryOwnerId, achievementId);
        user.WordsInDictionaryAmount++; 
        await _unitOfWork.SaveChangesAsync();
        
        var usersAchievements = await _unitOfWork.UsersAchievementsRepository.GetAsync(user.Id, achievementId);
        var nextLevelId = usersAchievements.CurrentLevel + 1;
        var level = await _unitOfWork.AchievementLevelRepository.GetById(achievementId, nextLevelId);
    }
}
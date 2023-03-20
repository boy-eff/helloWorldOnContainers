﻿using Achievements.Application.Contracts;
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
        var creatorAchievement = SeedData.CreatorAchievement;
        var achievementId = creatorAchievement.Id;
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.CreatorId);
        user.CollectionsCreatedAmount++;
        
        var achievementLevel = creatorAchievement.Levels
            .FirstOrDefault(x => x.PointsToAchieve == user.WordsInDictionaryAmount);

        if (achievementLevel is null)
        {
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        await _usersAchievementsService.UpdateUsersAchievementsLevelAsync(user, achievementId, achievementLevel);
    }
}
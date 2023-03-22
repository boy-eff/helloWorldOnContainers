﻿using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;

    public WordAddedToDictionaryMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.DictionaryOwnerId);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.WordsInDictionaryAmount++;
            await _unitOfWork.SaveChangesAsync();
        
            var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
            await _unitOfWork.CommitAsync();
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
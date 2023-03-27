using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordAddedToDictionaryMessageConsumer : IConsumer<WordAddedToDictionaryMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;
    private readonly ILogger<WordAddedToDictionaryMessageConsumer> _logger;

    public WordAddedToDictionaryMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService, ILogger<WordAddedToDictionaryMessageConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<WordAddedToDictionaryMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.DictionaryOwnerId);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.WordsInDictionaryAmount++;
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("User {UserId} added word {WordId} in dictionary, total count - {WordsCount}",
                context.Message.DictionaryOwnerId, context.Message.WordId, user.WordsInDictionaryAmount);
        
            var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
            await _unitOfWork.CommitAsync();
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        _logger.LogInformation("Achievement information was successfully updated for user {UserId}", user.Id);
    }
}
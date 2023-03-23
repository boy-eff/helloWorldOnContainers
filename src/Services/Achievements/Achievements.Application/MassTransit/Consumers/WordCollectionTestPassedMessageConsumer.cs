using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionTestPassedMessageConsumer : IConsumer<WordCollectionTestPassedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;
    private readonly ILogger<WordCollectionTestPassedMessageConsumer> _logger;

    public WordCollectionTestPassedMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService, ILogger<WordCollectionTestPassedMessageConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<WordCollectionTestPassedMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.UserId);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.CollectionTestsPassedAmount++;
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("User {UserId} passed collection {CollectionId} test, total passed tests count - {TestsCount}",
                user.Id, context.Message.WordCollectionId, user.CollectionTestsPassedAmount);
        
            await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
            await _unitOfWork.CommitAsync();
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        _logger.LogInformation("Achievement information successfully updated for user {UserId}", user.Id);
    }
}
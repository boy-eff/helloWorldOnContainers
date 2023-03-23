using Achievements.Application.Contracts;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class WordCollectionCreatedMessageConsumer : IConsumer<WordCollectionCreatedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersAchievementsService _usersAchievementsService;
    private readonly ILogger<WordCollectionCreatedMessageConsumer> _logger;

    public WordCollectionCreatedMessageConsumer(IUnitOfWork unitOfWork, IUsersAchievementsService usersAchievementsService, ILogger<WordCollectionCreatedMessageConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _usersAchievementsService = usersAchievementsService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<WordCollectionCreatedMessage> context)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(context.Message.CreatorId);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.CollectionsCreatedAmount++;
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("User {UserId} created collection {CollectionId}, total count - {CollectionsCount}",
                user.Id, context.Message.WordCollectionId, user.CollectionsCreatedAmount);
        
            var result = await _usersAchievementsService.UpsertUsersAchievementsLevelAsync(user, SeedData.ElderAchievement.Id);
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
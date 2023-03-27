using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserCreatedMessageConsumer> _logger;

    public UserCreatedMessageConsumer(IUnitOfWork unitOfWork, ILogger<UserCreatedMessageConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var user = context.Message.Adapt<User>();
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("User {UserId} has been successfully created", user.Id);
    }
}
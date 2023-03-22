using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using MassTransit;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserCreatedMessageConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var user = context.Message.Adapt<User>();
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
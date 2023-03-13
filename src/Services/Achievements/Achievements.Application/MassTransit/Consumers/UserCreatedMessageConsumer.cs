using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Mapster;
using MassTransit;

namespace Achievements.Application.MassTransit.Consumers;

public class UserCreatedMessageConsumer : IConsumer<IConsumer>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserCreatedMessageConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<IConsumer> context)
    {
        var user = context.Message.Adapt<User>();
        await _unitOfWork.UserRepository.AddAsync(user);
    }
}
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messages;
using Words.DataAccess;
using Words.DataAccess.Extensions;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MassTransit.Consumers;

public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
{
    private readonly WordsDbContext _dbContext;

    public UserCreatedMessageConsumer(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var user = context.Message.Adapt<User>();
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}
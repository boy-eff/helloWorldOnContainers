using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Messages;
using Words.DataAccess;
using Words.DataAccess.Extensions;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MassTransit.Consumers;

public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
{
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<UserCreatedMessageConsumer> _logger;

    public UserCreatedMessageConsumer(WordsDbContext dbContext, ILogger<UserCreatedMessageConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var user = context.Message.Adapt<User>();
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}
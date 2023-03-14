using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shared.Messages;
using Words.DataAccess;

namespace Words.BusinessAccess.Quartz.Jobs;

public class CheckForGameAnniversaryJob : IJob
{
    private readonly WordsDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public CheckForGameAnniversaryJob(WordsDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.Today;
        var users = await _dbContext.Users
            .Where(x => x.CreatedAt.Day == currentDate.Day && x.CreatedAt.Month == currentDate.Month)
            .ToListAsync();

        foreach (var user in users)
        {
            var yearsAmount = currentDate.Year - user.CreatedAt.Year;
            
            if (yearsAmount <= 0)
            {
                continue;
            }
            
            var message = new AppAnniversaryMessage()
            {
                UserId = user.Id,
                Years = yearsAmount
            };
            await _publishEndpoint.Publish(message);
        }
    }
}
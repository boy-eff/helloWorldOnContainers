﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shared.Messages;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Quartz.Jobs;

public class CheckForAppAnniversaryJob : IJob
{
    private readonly WordsDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public CheckForAppAnniversaryJob(WordsDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.Today;
        var users = new List<User>();
        
        AddLeapYearUsers(currentDate, users);
        
        users.AddRange(_dbContext.Users
                .Where(x => x.CreatedAt.Day == currentDate.Day && x.CreatedAt.Month == currentDate.Month)); 

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

    private void AddLeapYearUsers(DateTime date, List<User> users)
    {
        if (date.IsFirstOfMarch())
        {
            users.AddRange(_dbContext.Users
                .Where(x => x.CreatedAt.IsLastDayOfLeapFebruary()));
        }
    }
}
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using MassTransit;
using Shared.Messages;

namespace Achievements.Application.MassTransit.Consumers;

public class AppAnniversaryMessageConsumer : IConsumer<AppAnniversaryMessage>
{
    private readonly IUnitOfWork _unitOfWork;

    public AppAnniversaryMessageConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AppAnniversaryMessage> context)
    {
        var achievementLevel = SeedData.ElderAchievement.Levels.FirstOrDefault(x => x.PointsToAchieve == context.Message.Years);
        
        if (achievementLevel is null)
        {
            return;
        }

        var achievementId = SeedData.ElderAchievement.Id;
        var usersAchievement =
            await _unitOfWork.UsersAchievementsRepository.GetWithUserAsync(context.Message.UserId, achievementId);


        if (usersAchievement is null)
        {
            usersAchievement = new UsersAchievements()
            {
                AchievementId = achievementId,
                UserId = context.Message.UserId,
                CurrentLevel = achievementLevel.Level
            };
            _unitOfWork.UsersAchievementsRepository.Update(usersAchievement);
        }
        else
        {
            usersAchievement.CurrentLevel = achievementLevel.Level;
            await _unitOfWork.UsersAchievementsRepository.AddAsync(usersAchievement);
        }
        
        await _unitOfWork.SaveChangesAsync();
    }
}
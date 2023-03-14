using Quartz;
using Words.BusinessAccess.Quartz.Jobs;

namespace Words.WebAPI.Extensions;

public static class QuartzExtensions
{
    public static void ConfigureQuartz(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var updateViewsJobName = nameof(UpdateViewsJob);
            q.AddJob<UpdateViewsJob>(opt => opt.WithIdentity(updateViewsJobName));

            q.AddTrigger(opts => opts
                .ForJob(updateViewsJobName)
                .WithIdentity(updateViewsJobName + "Trigger")
                .WithCronSchedule(config["Quartz:UpdateViewsJob:Schedule"]));

            var updateDailyWordCollectionJobName = nameof(UpdateDailyWordCollectionJob);
            q.AddJob<UpdateDailyWordCollectionJob>(opt => opt.WithIdentity(updateDailyWordCollectionJobName));

            q.AddTrigger(opts => opts
                .ForJob(updateDailyWordCollectionJobName)
                .WithIdentity(updateDailyWordCollectionJobName + "Trigger")
                .WithCronSchedule(config["Quartz:UpdateDailyWordCollectionJob:Schedule"]));

            var checkForGameAnniversaryJobName = nameof(CheckForGameAnniversaryJob);
            q.AddJob<CheckForGameAnniversaryJob>(opt => opt.WithIdentity(checkForGameAnniversaryJobName));

            q.AddTrigger(opts => opts
                .ForJob(checkForGameAnniversaryJobName)
                .WithIdentity(checkForGameAnniversaryJobName + "Trigger")
                .WithCronSchedule(config["Quartz:CheckForGameAnniversaryJob:Schedule"]));
        });
        
        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });
    }
}
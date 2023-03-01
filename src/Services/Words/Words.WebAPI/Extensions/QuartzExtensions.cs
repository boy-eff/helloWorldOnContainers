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
            var updateDailyWordCollectionJobName = nameof(UpdateDailyWordCollectionJob);
            q.AddJob<UpdateViewsJob>(opt => opt.WithIdentity(updateViewsJobName));

            q.AddTrigger(opts => opts
                .ForJob(updateViewsJobName)
                .WithIdentity(updateViewsJobName + "Trigger")
                .WithCronSchedule(config["Quartz:UpdateViewsJob:Schedule"]));

            q.AddJob<UpdateDailyWordCollectionJob>(opt => opt.WithIdentity(updateDailyWordCollectionJobName));

            q.AddTrigger(opts => opts
                .ForJob(updateDailyWordCollectionJobName)
                .WithIdentity(updateDailyWordCollectionJobName + "Trigger")
                .WithCronSchedule(config["Quartz:UpdateDailyWordCollectionJob:Schedule"]));
        });
        
        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });
    }
}
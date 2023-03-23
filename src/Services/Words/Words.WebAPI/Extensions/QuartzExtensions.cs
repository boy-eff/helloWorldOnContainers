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

            var checkAppAnniversaryJobName = nameof(CheckForAppAnniversaryJob);
            q.AddJob<CheckForAppAnniversaryJob>(opt => opt.WithIdentity(checkAppAnniversaryJobName));

            q.AddTrigger(opts => opts
                .ForJob(checkAppAnniversaryJobName)
                .WithIdentity(checkAppAnniversaryJobName + "Trigger")
                .WithCronSchedule(config["Quartz:CheckForGameAnniversaryJob:Schedule"]));
            
            var updateCachedCollectionsJobName = nameof(UpdateCachedCollectionsJob);
            q.AddJob<UpdateCachedCollectionsJob>(opt => opt.WithIdentity(updateCachedCollectionsJobName));

            q.AddTrigger(opts => opts
                .ForJob(updateCachedCollectionsJobName)
                .WithIdentity(updateCachedCollectionsJobName + "Trigger")
                .WithCronSchedule(config["Quartz:UpdateCachedCollectionsJob:Schedule"]));
        });
        
        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });
    }
}
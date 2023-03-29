using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using Words.BusinessAccess.Quartz.Jobs;

namespace Words.UnitTests.Quartz;

public class JobExecutionContextProvider
{
    public static IJobExecutionContext GetJobExecutionContext()
    {
        var jobDetail = new JobDetailImpl("Default", typeof(CheckForAppAnniversaryJob));
        var scheduler = new Mock<IScheduler>();
        var firedBundle = new TriggerFiredBundle(jobDetail, new CronTriggerImpl(), null, false, DateTimeOffset.Now, null, null, null);
        var job = new Mock<IJob>();
        var context = new JobExecutionContextImpl(scheduler.Object, firedBundle, job.Object);
        return context;
    }
}
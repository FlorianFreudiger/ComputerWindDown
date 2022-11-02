using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace ComputerWindDown.Models.Jobs;

internal sealed class DailyJobWrapper<T> where T : IJob
{
    public IJobDetail JobDetail { get; }
    public ITrigger? Trigger { get; private set; }

    public DailyJobWrapper(JobDataMap jobDataMap)
    {
        JobDetail = JobBuilder.Create<T>()
            .UsingJobData(jobDataMap)
            .DisallowConcurrentExecution()
            .Build();
    }

    public async Task Start(bool failIfAlreadyStarted = true)
    {
        if (Trigger is null)
        {
            throw new InvalidOperationException("Time of day has not been set");
        }

        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        if (!await scheduler.CheckExists(JobDetail.Key))
        {
            await scheduler.ScheduleJob(JobDetail, Trigger);
        }
        else if (failIfAlreadyStarted)
        {
            throw new InvalidOperationException("Job was already scheduled");
        }
    }

    public async Task Stop(bool failIfAlreadyStopped = true)
    {
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        if (await scheduler.CheckExists(JobDetail.Key))
        {
            await scheduler.DeleteJob(JobDetail.Key);
        }
        else if (failIfAlreadyStopped)
        {
            throw new InvalidOperationException("Job was not scheduled");
        }
    }

    public async Task Restart(bool failIfAlreadyStopped = true)
    {
        await Stop(failIfAlreadyStopped);
        await Start(failIfAlreadyStopped);
    }

    public async Task TriggerNow()
    {
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.TriggerJob(JobDetail.Key);
    }

    public async Task SetTimeOfDay(TimeOfDay timeOfDay)
    {
        Trigger = BuildTrigger(timeOfDay);

        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        if (await scheduler.CheckExists(JobDetail.Key))
        {
            await Restart();
        }
    }

    public async Task SetTimeOfDay(TimeSpan timeOfDay)
    {
        await SetTimeOfDay(new TimeOfDay(timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds));
    }

    private ITrigger BuildTrigger(TimeOfDay timeOfDay)
    {
        return TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(x => x
                .OnEveryDay()
                .StartingDailyAt(timeOfDay)
                .EndingDailyAfterCount(1)
            )
            .Build();
    }
}

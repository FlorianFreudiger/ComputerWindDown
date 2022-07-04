using ComputerWindDown.Models.State.States;
using ComputerWindDown.Models.Time;
using ComputerWindDown.Properties;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.State
{
    internal static class EnabledStateSwitcher
    {
        public static EnabledState CreateNextState(StateManager stateManager)
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (TimeHelper.IsTimeOfDayBetween(now, start, end))
            {
                DateTime endUtc = TimeHelper.NextUtcDateWithLocalTimeOfDay(end);
                return new ActiveState(stateManager, endUtc);
            }
            else
            {
                DateTime endUtc = TimeHelper.NextUtcDateWithLocalTimeOfDay(start);
                return new InactiveState(stateManager, endUtc);
            }
        }

        private static readonly JobKey SwitchJobKey = new(nameof(SwitchEnabledStateJob), nameof(EnabledStateSwitcher));

        public static void Setup(StateManager stateManager)
        {
            IScheduler scheduler = stateManager.WindDown.Scheduler;

            // Assert this hasn't already been setup for this scheduler
            try
            {
                scheduler.ListenerManager.GetJobListener(nameof(SwitchEnabledStateJobListener));
                Debug.Assert(false);
            }
            catch (KeyNotFoundException) { }

            IJobListener jobListener = new SwitchEnabledStateJobListener(stateManager);
            scheduler.ListenerManager.AddJobListener(jobListener, KeyMatcher<JobKey>.KeyEquals(SwitchJobKey));
        }

        public static async Task ScheduleSwitch(IScheduler scheduler, DateTimeOffset switchTimeUtc)
        {
            Debug.Assert(await scheduler.GetJobDetail(SwitchJobKey) == null);

            IJobDetail job = JobBuilder.Create<SwitchEnabledStateJob>()
                .WithIdentity(SwitchJobKey)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(switchTimeUtc)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public static async Task CancelSwitch(IScheduler scheduler)
        {
            Debug.Assert(await scheduler.GetJobDetail(SwitchJobKey) != null);

            // Remove job, also removes its trigger
            await scheduler.DeleteJob(SwitchJobKey);
        }
    }

    internal class SwitchEnabledStateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }

    internal class SwitchEnabledStateJobListener : JobListenerSupport
    {
        private readonly StateManager _stateManager;

        internal SwitchEnabledStateJobListener(StateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public override string Name => nameof(SwitchEnabledStateJobListener);

        public override Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Previous state should be an EnabledState since switching to another state
            Debug.Assert(_stateManager.CurrentState is EnabledState);

            // Create and switch to new EnabledState
            EnabledState newState = EnabledStateSwitcher.CreateNextState(_stateManager);
            _stateManager.ChangeState(newState);

            return base.JobWasExecuted(context, jobException, cancellationToken);
        }
    }
}

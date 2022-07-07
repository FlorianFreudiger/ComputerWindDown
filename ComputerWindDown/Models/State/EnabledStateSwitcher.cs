using ComputerWindDown.Models.State.States;
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
        private static readonly JobKey SwitchJobKey = new(nameof(SwitchEnabledStateJob), nameof(EnabledStateSwitcher));

        public static void Setup(StateManager stateManager)
        {
            IScheduler scheduler = stateManager.WindDown.Scheduler;

            // Ensure this hasn't already been setup for this scheduler
            try
            {
                scheduler.ListenerManager.GetJobListener(nameof(SwitchEnabledStateJobListener));

                throw new InvalidOperationException("StateManager already has job listener registered");
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
            _stateManager.RefreshState();
            Debug.Assert(_stateManager.CurrentState is EnabledState);

            return base.JobWasExecuted(context, jobException, cancellationToken);
        }
    }
}

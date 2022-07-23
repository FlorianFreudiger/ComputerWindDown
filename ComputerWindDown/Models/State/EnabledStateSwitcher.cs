using ComputerWindDown.Models.State.States;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.State
{
    internal class EnabledStateSwitcher
    {
        private static readonly JobKey SwitchJobKey = new(nameof(SwitchEnabledStateJob), nameof(EnabledStateSwitcher));

        private IScheduler? _scheduler;
        private readonly StateManager _stateManager;

        public EnabledStateSwitcher(StateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task Initialize()
        {
            // 1. Setup scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = await schedulerFactory.GetScheduler();

            await _scheduler.Start();

            IJobListener jobListener = new SwitchEnabledStateJobListener(this);
            _scheduler.ListenerManager.AddJobListener(jobListener, KeyMatcher<JobKey>.KeyEquals(SwitchJobKey));
        }

        public async Task ScheduleSwitch(DateTimeOffset switchTimeUtc)
        {
            Debug.Assert(_scheduler != null);
            Debug.Assert(await _scheduler.GetJobDetail(SwitchJobKey) == null);

            IJobDetail job = JobBuilder.Create<SwitchEnabledStateJob>()
                .WithIdentity(SwitchJobKey)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(switchTimeUtc)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task CancelSwitch()
        {
            Debug.Assert(_scheduler != null);
            Debug.Assert(await _scheduler.GetJobDetail(SwitchJobKey) != null);

            // Remove job, also removes its trigger
            await _scheduler.DeleteJob(SwitchJobKey);
        }

        public void SwitchState()
        {
            // Previous state should be an EnabledState since switching to another state
            Debug.Assert(_stateManager.CurrentState is EnabledState);

            // Create and switch to new EnabledState
            _stateManager.RefreshState();
            Debug.Assert(_stateManager.CurrentState is EnabledState);
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
        private readonly EnabledStateSwitcher _enabledStateSwitcher;

        internal SwitchEnabledStateJobListener(EnabledStateSwitcher enabledStateSwitcher)
        {
            _enabledStateSwitcher = enabledStateSwitcher;
        }

        public override string Name => nameof(SwitchEnabledStateJobListener);

        public override Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new())
        {
            _enabledStateSwitcher.SwitchState();

            return base.JobWasExecuted(context, jobException, cancellationToken);
        }
    }
}

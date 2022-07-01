using ComputerWindDown.Properties;
using Quartz;
using Quartz.Impl;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.Time
{
    internal class Coordinator
    {
        public Subject<double> TransitionProgress;

        private IScheduler? _scheduler;

        public Coordinator()
        {
            TransitionProgress = new Subject<double>();
            Settings.Default.PropertyChanged += SettingsChanged;
        }

        private void SettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _ = Run();
        }

        public virtual async Task Run()
        {
            if (_scheduler == null)
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                _scheduler = await schedulerFactory.GetScheduler();
            }

            await _scheduler.Clear();

            // Create empty job
            IJobDetail job = JobBuilder.Create<UpdateJob>()
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()
                )
                .StartNow()
                .Build();

            DateTimeOffset? ft = await _scheduler.ScheduleJob(job, trigger);

            // Listen for job executions
            IJobListener updateJobListener = new UpdateJobListener(this);
            _scheduler.ListenerManager.AddJobListener(updateJobListener);

            await _scheduler.Start();
        }
    }
}

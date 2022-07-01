using ComputerWindDown.Properties;
using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.Time
{
    internal class UpdateJobListener : JobListenerSupport
    {
        private readonly Coordinator _coordinator;

        public UpdateJobListener(Coordinator coordinator)
        {
            _coordinator = coordinator;
        }

        public void Update()
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;
            TimeSpan now = DateTime.Now.TimeOfDay;

            double ratio = TimeHelper.TimeOfDayRangeProgression(now, start, end);
            if (ratio < 0)
            {
                ratio = 0;
            }

            _coordinator.TransitionProgress.OnNext(ratio);
        }

        public override string Name => nameof(UpdateJobListener);

        public override Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Update();

            return base.JobWasExecuted(context, jobException, cancellationToken);
        }
    }
}

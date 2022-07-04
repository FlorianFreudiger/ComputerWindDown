using Quartz;
using System;

namespace ComputerWindDown.Models.State.States
{
    internal abstract class EnabledState : WindDownState
    {
        public readonly DateTime StartUtc;
        public readonly DateTime EndUtc;

        protected EnabledState(StateManager stateManager, DateTime endUtc) : base(stateManager)
        {
            StartUtc = DateTime.UtcNow;
            EndUtc = endUtc;
        }

        public override void Activate(WindDownState previousState)
        {
            base.Activate(previousState);

            IScheduler scheduler = StateManager.WindDown.Scheduler;
            _ = EnabledStateSwitcher.ScheduleSwitch(scheduler, EndUtc);
        }

        public override void Deactivate(WindDownState newState)
        {
            base.Deactivate(newState);

            IScheduler scheduler = StateManager.WindDown.Scheduler;
            _ = EnabledStateSwitcher.CancelSwitch(scheduler);
        }
    }
}

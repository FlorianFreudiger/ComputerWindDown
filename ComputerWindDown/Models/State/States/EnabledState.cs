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

            _ = StateManager.EnabledStateSwitcher.ScheduleSwitch(EndUtc);
        }

        public override void Deactivate(WindDownState newState)
        {
            base.Deactivate(newState);

            _ = StateManager.EnabledStateSwitcher.CancelSwitch();
        }
    }
}

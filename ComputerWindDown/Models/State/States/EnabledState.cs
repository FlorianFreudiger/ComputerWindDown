using ComputerWindDown.Models.Time;
using ComputerWindDown.Properties;
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

        public static EnabledState CreateEnabledState(StateManager stateManager)
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
    }
}

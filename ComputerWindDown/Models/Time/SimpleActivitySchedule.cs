using ComputerWindDown.Properties;
using System;

namespace ComputerWindDown.Models.Time
{
    internal class SimpleActivitySchedule : IActivitySchedule
    {
        public DateTime GetNextSwitchTimeUtc()
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (TimeHelper.IsTimeOfDayBetween(now, start, end))
            {
                return TimeHelper.NextUtcDateWithLocalTimeOfDay(end);
            }
            else
            {
                return TimeHelper.NextUtcDateWithLocalTimeOfDay(start);
            }
        }

        public bool IsActiveForTime(DateTime time, bool utc)
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;

            if (utc)
            {
                time = time.ToLocalTime();
            }

            return TimeHelper.IsTimeOfDayBetween(time.TimeOfDay, start, end);
        }
    }
}

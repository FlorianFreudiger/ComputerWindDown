using System;
using ComputerWindDown.Properties;

namespace ComputerWindDown.Models.Time
{
    internal class SimpleActivitySchedule : IActivitySchedule
    {
        public DateTime GetNextSwitchTimeUtc()
        {
            TimeSpan start = Settings.Default.StartTime;
            TimeSpan end = Settings.Default.EndTime;
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
            TimeSpan start = Settings.Default.StartTime;
            TimeSpan end = Settings.Default.EndTime;

            if (utc)
            {
                time = time.ToLocalTime();
            }

            return TimeHelper.IsTimeOfDayBetween(time.TimeOfDay, start, end);
        }
    }
}

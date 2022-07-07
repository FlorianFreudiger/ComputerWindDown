using System;

namespace ComputerWindDown.Models.Time
{
    internal interface IActivitySchedule
    {
        public DateTime GetNextSwitchTimeUtc();

        public bool IsActiveForTime(DateTime time, bool utc);
    }
}

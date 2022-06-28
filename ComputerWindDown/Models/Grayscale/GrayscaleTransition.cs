using ComputerWindDown.Properties;
using System;
using System.Diagnostics;

namespace ComputerWindDown.Models.Grayscale
{
    internal abstract class GrayscaleTransition
    {
        public GrayscaleTransition()
        {
            Settings.Default.PropertyChanged += SettingsChanged;
        }

        private void SettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // TODO: Scheduling
            Update();
        }

        public void Update()
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;
            TimeSpan now = DateTime.Now.TimeOfDay;

            double ratio = TimeOfDayRangeProgression(now, start, end);
            if (ratio > 0)
            {
                SetTransition(ratio);
            }
            else
            {
                SetTransition(0);
            }
        }

        public abstract void SetTransition(double progress);

        private static bool IsTimeOfDayBetween(TimeSpan targetTime, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime < startTime)
            {
                return !(targetTime > endTime && targetTime < startTime);
            }
            return targetTime >= startTime && targetTime <= endTime;
        }

        private static TimeSpan TimeOfDayRangeDuration(TimeSpan fromTime, TimeSpan toTime)
        {
            if (fromTime.Days != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fromTime), "TimeSpan has to be time of day!");
            }
            if (toTime.Days != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toTime), "TimeSpan has to be time of day!");
            }


            if (fromTime > toTime)
            {
                // If start and end are not on the same day we have to add 24 hours so the end wraps around midnight
                toTime += TimeSpan.FromDays(1);
            }

            return toTime.Subtract(fromTime);
        }

        private static double TimeOfDayRangeProgression(TimeSpan targetTime, TimeSpan startTime, TimeSpan endTime)
        {
            TimeSpan rangeDuration = TimeOfDayRangeDuration(startTime, endTime);
            TimeSpan targetDifference = TimeOfDayRangeDuration(startTime, targetTime);

            double ratio = targetDifference.Divide(rangeDuration);
            Debug.Assert(ratio >= 0);

            // If the ratio is over 1 then the time is outside the range, return -1
            if (ratio > 1)
            {
                Debug.Assert(!IsTimeOfDayBetween(targetTime, startTime, endTime));
                return -1;
            }

            Debug.Assert(IsTimeOfDayBetween(targetTime, startTime, endTime));
            return ratio;
        }
    }
}

using System;
using System.Diagnostics;

namespace ComputerWindDown.Models.Time;

internal static class TimeHelper
{
    public static bool IsTimeOfDayBetween(TimeSpan targetTime, TimeSpan startTime, TimeSpan endTime)
    {
        if (endTime < startTime)
        {
            return !(targetTime > endTime && targetTime < startTime);
        }

        return targetTime >= startTime && targetTime <= endTime;
    }

    public static TimeSpan TimeOfDayRangeDuration(TimeSpan fromTime, TimeSpan toTime)
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

    public static double TimeOfDayRangeProgression(TimeSpan targetTime, TimeSpan startTime, TimeSpan endTime)
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

    public static DateTime NextUtcDateWithLocalTimeOfDay(TimeSpan localTimeOfDay)
    {
        DateTime date = DateTime.Now.Date;
        date += localTimeOfDay;

        if (date < DateTime.Now)
        {
            date = date.AddDays(1);
        }

        Debug.Assert(date >= DateTime.Now);

        return date.ToUniversalTime();
    }
}

using System;
using System.Reactive.Subjects;
using ComputerWindDown.Properties;

namespace ComputerWindDown.Models.Time
{
    internal class Coordinator
    {
        public Subject<double> TransitionProgress;

        public Coordinator()
        {
            TransitionProgress = new Subject<double>();
            Settings.Default.PropertyChanged += SettingsChanged;
        }

        private void SettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            TimeSpan start = Settings.Default.TransitionStart;
            TimeSpan end = Settings.Default.TransitionEnd;
            TimeSpan now = DateTime.Now.TimeOfDay;

            double ratio = TimeHelper.TimeOfDayRangeProgression(now, start, end);
            if (ratio > 0)
            {
                TransitionProgress.OnNext(ratio);
            }
            else
            {
                TransitionProgress.OnNext(0.0);
            }
        }
    }
}

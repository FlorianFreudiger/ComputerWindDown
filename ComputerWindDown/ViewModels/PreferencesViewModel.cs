using ComputerWindDown.Properties;
using ReactiveUI;
using System;

namespace ComputerWindDown.ViewModels
{
    public class PreferencesViewModel : ViewModelBase
    {
        public TimeSpan TransitionStart
        {
            get => Settings.Default.TransitionStart;
            set
            {
                Settings.Default.TransitionStart = value;
                Settings.Default.Save();
                this.RaisePropertyChanged(nameof(TransitionStart));
            }
        }
        public TimeSpan TransitionEnd
        {
            get => Settings.Default.TransitionEnd;
            set
            {
                Settings.Default.TransitionEnd = value;
                Settings.Default.Save();
                this.RaisePropertyChanged(nameof(TransitionEnd));
            }
        }
    }
}

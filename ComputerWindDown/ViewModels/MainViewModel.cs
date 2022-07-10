using ComputerWindDown.Properties;
using ReactiveUI;
using System;

namespace ComputerWindDown.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public bool Enable
        {
            get => Settings.Default.Enable;
            set
            {
                Settings.Default.Enable = value;
                Settings.Default.Save();
                this.RaisePropertyChanged(nameof(Enable));
            }
        }
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

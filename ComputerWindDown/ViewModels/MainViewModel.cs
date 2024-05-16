using System;
using ComputerWindDown.Models;
using ComputerWindDown.Properties;
using ReactiveUI;

namespace ComputerWindDown.ViewModels;

public class MainViewModel : ViewModelBase
{
    public bool Enable
    {
        get => Settings.Default.Enable;
        set
        {
            Settings.Default.Enable = value;
            Settings.Default.Save();
            this.RaisePropertyChanged();
        }
    }

    public TimeSpan StartTime
    {
        get => Settings.Default.StartTime;
        set
        {
            Settings.Default.StartTime = value;
            Settings.Default.Save();
            this.RaisePropertyChanged();
        }
    }

    public TimeSpan EndTime
    {
        get => Settings.Default.EndTime;
        set
        {
            Settings.Default.EndTime = value;
            Settings.Default.Save();
            this.RaisePropertyChanged();
        }
    }

    public bool MinimizeToTray
    {
        get => Settings.Default.MinimizeToTray;
        set
        {
            Settings.Default.MinimizeToTray = value;
            Settings.Default.Save();
            this.RaisePropertyChanged();
        }
    }

    public Autostart AutostartInstance => Autostart.Instance;
}

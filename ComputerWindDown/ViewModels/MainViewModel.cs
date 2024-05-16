using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ComputerWindDown.Models;
using ComputerWindDown.Properties;

namespace ComputerWindDown.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool _enable = Settings.Default.Enable;

    partial void OnEnableChanged(bool value)
    {
        Settings.Default.Enable = value;
        Settings.Default.Save();
    }

    [ObservableProperty] private TimeSpan _startTime = Settings.Default.StartTime;

    partial void OnStartTimeChanged(TimeSpan value)
    {
        Settings.Default.StartTime = value;
        Settings.Default.Save();
    }

    [ObservableProperty] private TimeSpan _endTime = Settings.Default.EndTime;

    partial void OnEndTimeChanged(TimeSpan value)
    {
        Settings.Default.EndTime = value;
        Settings.Default.Save();
    }

    [ObservableProperty] private bool _minimizeToTray = Settings.Default.MinimizeToTray;

    partial void OnMinimizeToTrayChanged(bool value)
    {
        Settings.Default.MinimizeToTray = value;
        Settings.Default.Save();
    }

    public Autostart AutostartInstance => Autostart.Instance;
}

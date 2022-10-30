using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using StartupHelper;

namespace ComputerWindDown.Models;

public sealed class Autostart : INotifyPropertyChanged
{
    // TODO?: Thread-safe Singleton, if necessary
    public static readonly Autostart Instance = new();

    private readonly StartupManager? _startupManager;

    public bool IsAvailable => _startupManager != null;

    public bool RunAtStartup
    {
        get => _startupManager?.IsRegistered ?? false;
        set
        {
            var startupManager = _startupManager ?? throw new InvalidOperationException("Autostart is not available");

            if (value == startupManager.IsRegistered) return;
            if (value)
            {
                startupManager.Register();
            }
            else
            {
                startupManager.Unregister();
            }
            OnPropertyChanged();
        }
    }

    public bool WasAutoStarted => _startupManager?.IsStartedUp ?? false;

    private Autostart()
    {
        string? path = FindExecutablePath();

        if (path != null)
        {
            _startupManager = new StartupManager(path, nameof(ComputerWindDown), RegistrationScope.Local, false);
        }
    }

    private static string? FindExecutablePath()
    {
        // 1. Find the path to the executable

        // Use ProcessPath since Assembly.Location is not supported in single-file deployments
        // https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#api-incompatibility
        string? path = Environment.ProcessPath;

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        // 2. Verify path is executable
        if (path.EndsWith(".exe") && File.Exists(path))
        {
            return path;
        }

        return null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

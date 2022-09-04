using System;
using System.IO;
using StartupHelper;

namespace ComputerWindDown.Models;

public static class Autostart
{
    private static readonly StartupManager? StartupManager;

    public static bool IsAvailable => StartupManager != null;

    public static bool RunAtStartup
    {
        get => StartupManager?.IsRegistered ?? false;
        set
        {
            var startupManager = StartupManager ?? throw new InvalidOperationException("Autostart is not available");

            if (value == startupManager.IsRegistered) return;
            if (value)
            {
                startupManager.Register();
            }
            else
            {
                startupManager.Unregister();
            }
        }
    }

    public static bool WasAutoStarted => StartupManager?.IsStartedUp ?? false;

    static Autostart()
    {
        string? path = FindExecutablePath();

        if (path != null)
        {
            StartupManager = new StartupManager(path, nameof(ComputerWindDown), RegistrationScope.Local, false);
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
}

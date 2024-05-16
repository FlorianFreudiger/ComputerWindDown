using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ComputerWindDown.Models;
using ComputerWindDown.Properties;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views;

namespace ComputerWindDown;

public class App : Application
{
    private WindDown? _windDown;

    public App()
    {
        DataContext = new AppViewModel(this);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _windDown = new WindDown();
            _ = _windDown.Initialize();


            void UpdateShutdownMode() => desktop.ShutdownMode = Settings.Default.MinimizeToTray
                ? ShutdownMode.OnExplicitShutdown
                : ShutdownMode.OnLastWindowClose;

            // Update now and subscribe to future changes
            UpdateShutdownMode();
            Settings.Default.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(Settings.Default.MinimizeToTray))
                {
                    UpdateShutdownMode();
                }
            };

            desktop.ShutdownRequested += ShutdownRequested;
        }

        if (!Autostart.Instance.WasAutoStarted)
        {
            ShowMainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        Debug.Assert(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime);

        Debug.Assert(_windDown != null);
        _ = _windDown.ScreenEffectJobsDirector.Stop();
        _windDown.ScreenEffectController.Reset();
    }

    public void ShowMainWindow()
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Check if MainWindow hasn't been created or is closed
            if (desktop.MainWindow?.PlatformImpl == null)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = DataContext,
                };
            }

            desktop.MainWindow.Show();
            desktop.MainWindow.Activate();
        }
    }

    public void Exit()
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.TryShutdown();
        }
    }
}

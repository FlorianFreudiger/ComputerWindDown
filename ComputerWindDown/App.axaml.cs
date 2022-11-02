using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ComputerWindDown.Models;
using ComputerWindDown.Properties;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views;
using ReactiveUI;

namespace ComputerWindDown
{
    public partial class App : Application
    {
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
                WindDown windDown = new WindDown();
                _ = windDown.Initialize();

                Settings.Default.WhenAnyValue(x => x.MinimizeToTray)
                    .Select(x => x ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnLastWindowClose)
                    .Subscribe(x => desktop.ShutdownMode = x);
            }

            if (!Autostart.Instance.WasAutoStarted)
            {
                ShowMainWindow();
            }

            base.OnFrameworkInitializationCompleted();
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
                        DataContext = DataContext
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
}

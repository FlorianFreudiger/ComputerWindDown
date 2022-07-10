using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ComputerWindDown.Models;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views;

namespace ComputerWindDown
{
    public partial class App : Application
    {
        public App()
        {
            DataContext = new AppViewModel();
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

                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }

            CreateOrShowMainWindow();

            base.OnFrameworkInitializationCompleted();
        }

        public static void CreateOrShowMainWindow()
        {
            if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Check if MainWindow hasn't been created or is closed
                if (desktop.MainWindow?.PlatformImpl == null)
                {
                    var preferencesViewModel = new PreferencesViewModel();
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel(preferencesViewModel),
                    };
                }

                desktop.MainWindow.Show();
                desktop.MainWindow.Activate();
            }
        }
    }
}

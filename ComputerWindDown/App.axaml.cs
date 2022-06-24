using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views;

namespace ComputerWindDown
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var preferencesViewModel = new PreferencesViewModel();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(preferencesViewModel),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}

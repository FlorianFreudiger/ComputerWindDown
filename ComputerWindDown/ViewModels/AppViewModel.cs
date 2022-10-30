using System.Reactive;
using System.Reactive.Linq;
using ComputerWindDown.Models;
using ReactiveUI;

namespace ComputerWindDown.ViewModels
{
    internal class AppViewModel : ViewModelBase
    {
        public string Name => "Wind Down";

        public AppViewModel(App app)
        {
            // Create simple exit and show window commands
            ExitCommand = ReactiveCommand.Create(app.Exit);
            ShowCommand = ReactiveCommand.Create(app.ShowMainWindow);

            // Create toggle auto-start command and bind canExecute to IsAvailable to disable the menu item if it is not available
            var canToggleAutostart = Autostart.Instance.WhenAnyValue(autostart => autostart.IsAvailable);
            ToggleAutostartCommand = ReactiveCommand.Create(ToggleAutostart, canToggleAutostart);

            // Create string property label for the toggle auto-start menu item
            Autostart.Instance.WhenAnyValue(autostart => autostart.RunAtStartup)
                .Select(runAtStartup => runAtStartup ? "Do not run at startup" : "Run at startup")
                .ToProperty(this, appViewModel => appViewModel.ToggleAutostartString, out _toggleAutostartString);
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ReactiveCommand<Unit, Unit> ShowCommand { get; }

        public ReactiveCommand<Unit, Unit> ToggleAutostartCommand { get; }

        private readonly ObservableAsPropertyHelper<string> _toggleAutostartString;
        public string ToggleAutostartString => _toggleAutostartString.Value;

        private void ToggleAutostart()
        {
            Autostart.Instance.RunAtStartup = !Autostart.Instance.RunAtStartup;
        }
    }
}

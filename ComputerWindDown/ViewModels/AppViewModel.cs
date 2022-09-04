using System.Reactive;
using ComputerWindDown.Models;
using ReactiveUI;

namespace ComputerWindDown.ViewModels
{
    internal class AppViewModel : ViewModelBase
    {
        public string Name => "Wind Down";

        public AppViewModel(App app)
        {
            ExitCommand = ReactiveCommand.Create(app.Exit);
            ShowCommand = ReactiveCommand.Create(app.ShowMainWindow);
            ToggleAutostartCommand = ReactiveCommand.Create(ToggleAutostart);
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ReactiveCommand<Unit, Unit> ShowCommand { get; }

        public ReactiveCommand<Unit, Unit> ToggleAutostartCommand { get; }

        public string ToggleAutostartString => Autostart.RunAtStartup ? "Do not run at startup" : "Run at startup";

        public bool ToggleAutostartEnabled => Autostart.IsAvailable;

        private void ToggleAutostart()
        {
            Autostart.RunAtStartup = !Autostart.RunAtStartup;
            this.RaisePropertyChanged(nameof(ToggleAutostartString));
        }
    }
}

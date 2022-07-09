using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.Reactive;

namespace ComputerWindDown.ViewModels
{
    internal class AppViewModel : ViewModelBase
    {
        public string Name => "Wind Down";

        public AppViewModel()
        {
            ExitCommand = ReactiveCommand.Create(Exit);
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public void Exit()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.TryShutdown();
            }
        }
    }
}

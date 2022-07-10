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
            ShowCommand = ReactiveCommand.Create(Show);
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public void Exit()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.TryShutdown();
            }
        }

        public ReactiveCommand<Unit, Unit> ShowCommand { get; }
        public void Show()
        {
            App.CreateOrShowMainWindow();
        }
    }
}

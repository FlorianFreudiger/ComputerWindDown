using System.Reactive;
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
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ReactiveCommand<Unit, Unit> ShowCommand { get; }
    }
}

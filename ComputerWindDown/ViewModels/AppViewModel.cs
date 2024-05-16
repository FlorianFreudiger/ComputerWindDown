using CommunityToolkit.Mvvm.Input;
using ComputerWindDown.Models;

namespace ComputerWindDown.ViewModels;

internal partial class AppViewModel : ViewModelBase
{
    private readonly App _app;

    public AppViewModel(App app)
    {
        _app = app;

        // Update the string inside the tray menu when the autostart property changes
        Autostart.Instance.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(Autostart.Instance.RunAtStartup))
            {
                OnPropertyChanged(nameof(ToggleAutostartString));
            }
        };
    }

    // Cannot be static to allow for data binding
    public string Name => "Wind Down";

    public string ToggleAutostartString => Autostart.Instance.RunAtStartup ? "Do not run at startup" : "Run at startup";

    [RelayCommand]
    private void ExitApp() => _app.Exit();

    [RelayCommand]
    private void ShowApp() => _app.ShowMainWindow();

    [RelayCommand(CanExecute = nameof(CanToggleAutostart))]
    private void ToggleAutostart() => Autostart.Instance.RunAtStartup = !Autostart.Instance.RunAtStartup;

    private static bool CanToggleAutostart() => Autostart.Instance.IsAvailable;
}

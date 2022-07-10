using Avalonia.Controls;
using ComputerWindDown.ViewModels;

namespace ComputerWindDown.Views
{
    public partial class PreferencesView : UserControl
    {
        public PreferencesView()
        {
            InitializeComponent();

            DataContext = new PreferencesViewModel();
        }
    }
}

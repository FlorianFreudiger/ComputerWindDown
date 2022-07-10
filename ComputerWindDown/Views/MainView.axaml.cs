using Avalonia.Controls;
using ComputerWindDown.ViewModels;

namespace ComputerWindDown.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}

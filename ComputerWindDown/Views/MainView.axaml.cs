using System;
using Avalonia.Controls;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views.Pages;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;

namespace ComputerWindDown.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            NavigationView.SelectionChanged += NavigationView_SelectionChanged;
            NavigationView.SelectedItem = NavigationView.MenuItems.ElementAt(0);
        }

        private void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
        {
            NavigationView.Content = e.SelectedItemContainer.Tag switch
            {
                "time" => new TimePage(),
                "screen-effect" => new ScreenEffectPage(),
                _ => throw new ArgumentException("Unknown tag")
            };
            NavigationView.Header = e.SelectedItemContainer.Content;
        }
    }
}

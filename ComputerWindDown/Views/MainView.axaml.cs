using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using ComputerWindDown.ViewModels;
using ComputerWindDown.Views.Pages;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;

namespace ComputerWindDown.Views
{
    public partial class MainView : UserControl
    {
        private readonly Dictionary<string, UserControl> _pages = new()
        {
            { "time", new TimePage() },
            { "screen-effect", new ScreenEffectPage() },
            { "settings", new SettingsPage() }
        };

        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            NavigationView.SelectionChanged += NavigationView_SelectionChanged;
            NavigationView.SelectedItem = NavigationView.MenuItems.ElementAt(0);
        }

        private void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.IsSettingsSelected)
            {
                NavigationView.Header = "Settings";
                NavigationView.Content = _pages["settings"];
            }
            else
            {
                NavigationView.Header = e.SelectedItemContainer.Content;

                var tag = e.SelectedItemContainer.Tag as string;
                Debug.Assert(tag != null);
                NavigationView.Content = _pages[tag];
            }
        }
    }
}

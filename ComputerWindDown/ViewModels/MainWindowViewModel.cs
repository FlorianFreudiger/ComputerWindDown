using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerWindDown.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PreferencesViewModel Preferences { get; }

        public MainWindowViewModel(PreferencesViewModel preferencesViewModel)
        {
            Preferences = preferencesViewModel;
        }
    }
}

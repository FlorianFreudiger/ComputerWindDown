using ComputerWindDown.Models.State.States;
using ComputerWindDown.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.State
{
    internal class StateManager
    {
        public readonly WindDown WindDown;
        public readonly EnabledStateSwitcher EnabledStateSwitcher;

        public WindDownState CurrentState;

        public StateManager(WindDown windDown)
        {
            WindDown = windDown;
            CurrentState = new StartupState(this);

            Settings.Default.PropertyChanged += SettingsChanged;

            EnabledStateSwitcher = new EnabledStateSwitcher(this);
        }

        public async Task Initialize()
        {
            await EnabledStateSwitcher.Initialize();

            // StartupState will switch to first real state
            Debug.Assert(CurrentState is StartupState);
            CurrentState.Activate(CurrentState);
        }

        public void ChangeState(WindDownState newState)
        {
            WindDownState oldState = CurrentState;
            oldState.Deactivate(newState);

            CurrentState = newState;
            newState.Activate(oldState);

            Debug.WriteLine("Switched from " + oldState + " to " + newState);
        }

        private void SettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            RefreshState();
        }

        public void RefreshState()
        {
            ChangeState(CreateState());
        }

        private WindDownState CreateState()
        {
            if (!Settings.Default.Enable)
            {
                return new DisabledState(this);
            }

            DateTime now = DateTime.Now;
            DateTime nextEndTimeUtc = WindDown.ActivitySchedule.GetNextSwitchTimeUtc();
            if (WindDown.ActivitySchedule.IsActiveForTime(now, false))
            {
                return new ActiveState(this, nextEndTimeUtc);
            }
            else
            {
                return new InactiveState(this, nextEndTimeUtc);
            }
        }
    }
}

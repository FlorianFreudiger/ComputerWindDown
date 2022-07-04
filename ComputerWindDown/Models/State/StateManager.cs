using System.Diagnostics;
using ComputerWindDown.Models.State.States;

namespace ComputerWindDown.Models.State
{
    internal class StateManager
    {
        public readonly WindDown WindDown;

        public WindDownState CurrentState;

        public StateManager(WindDown windDown)
        {
            WindDown = windDown;
            CurrentState = new StartupState(this);

            // StartupState will initialize WindDown scheduler, EnabledStateSwitcher, etc
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
    }
}

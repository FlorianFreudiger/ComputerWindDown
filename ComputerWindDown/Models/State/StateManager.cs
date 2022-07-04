using ComputerWindDown.Models.State.States;
using System.Diagnostics;

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
        }

        public void Initialize()
        {
            EnabledStateSwitcher.Setup(this);

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
    }
}

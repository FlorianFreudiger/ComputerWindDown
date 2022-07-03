using System;

namespace ComputerWindDown.Models.State.States
{
    internal class InactiveState : EnabledState
    {
        public InactiveState(StateManager stateManager, DateTime endUtc) : base(stateManager, endUtc)
        {
        }

        // Disabling screen effects is handled by ActiveState's deactivation
    }
}

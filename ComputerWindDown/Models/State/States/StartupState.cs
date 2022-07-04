using ComputerWindDown.Properties;

namespace ComputerWindDown.Models.State.States
{
    internal class StartupState : WindDownState
    {
        public StartupState(StateManager stateManager) : base(stateManager)
        {
        }

        public override void Activate(WindDownState previousState)
        {
            base.Activate(previousState);

            // Switch to first real state
            WindDownState newState;
            if (!Settings.Default.Enable)
            {
                newState = new DisabledState(StateManager);
            }
            else
            {
                newState = EnabledStateSwitcher.CreateNextState(StateManager);
            }

            StateManager.ChangeState(newState);
        }
    }
}

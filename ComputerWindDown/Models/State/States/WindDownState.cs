namespace ComputerWindDown.Models.State.States
{
    internal abstract class WindDownState
    {
        protected StateManager StateManager;

        protected WindDownState(StateManager stateManager)
        {
            StateManager = stateManager;
        }

        public virtual void Activate(WindDownState previousState) { }

        public virtual void Deactivate(WindDownState newState) { }
    }
}

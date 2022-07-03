using System;

namespace ComputerWindDown.Models.State.States
{
    internal class ActiveState : EnabledState
    {
        public ActiveState(StateManager stateManager, DateTime endUtc) : base(stateManager, endUtc)
        {
        }

        public override void Activate(WindDownState previousState)
        {
            base.Activate(previousState);

            StateManager.WindDown.GrayscaleTransition.SetTransition(1);
        }

        public override void Deactivate(WindDownState newState)
        {
            base.Deactivate(newState);

            if (newState is not ActiveState)
            {
                StateManager.WindDown.GrayscaleTransition.SetTransition(0);
            }
        }
    }
}

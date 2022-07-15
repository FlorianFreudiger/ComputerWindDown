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

            StateManager.WindDown.ScreenEffect.SetActive(true);
        }

        public override void Deactivate(WindDownState newState)
        {
            base.Deactivate(newState);

            if (newState is not ActiveState)
            {
                StateManager.WindDown.ScreenEffect.SetActive(false);
            }
        }
    }
}

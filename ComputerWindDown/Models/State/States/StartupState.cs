using System.Threading.Tasks;
using ComputerWindDown.Properties;
using Quartz;
using Quartz.Impl;

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

            _ = Initialize();
        }

        private async Task Initialize()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            StateManager.WindDown.Scheduler = await schedulerFactory.GetScheduler();

            WindDownState newState;
            if (!Settings.Default.Enable)
            {
                newState = new DisabledState(StateManager);
            }
            else
            {
                newState = EnabledState.CreateEnabledState(StateManager);
            }

            StateManager.ChangeState(newState);
        }
    }
}

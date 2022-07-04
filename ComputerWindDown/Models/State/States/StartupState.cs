using ComputerWindDown.Properties;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

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
            // 1. Setup scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            StateManager.WindDown.Scheduler = scheduler;

            EnabledStateSwitcher.Setup(StateManager);

            await scheduler.Start();


            // 2. Switch to first job
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

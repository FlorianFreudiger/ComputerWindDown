using ComputerWindDown.Models.ScreenEffects;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly StateManager StateManager;
        public readonly ScreenEffect ScreenEffect;

        private IScheduler? _scheduler;
        public IScheduler Scheduler
        {
            get
            {
                // Scheduler should exist, initialized by StartupState
                Debug.Assert(_scheduler != null);
                return _scheduler;
            }
            set
            {
                // Existing scheduler shouldn't be replaced
                Debug.Assert(_scheduler == null);
                _scheduler = value;
            }
        }

        public IActivitySchedule ActivitySchedule;

        public WindDown()
        {
            ScreenEffect = new NvidiaDigitalVibranceTransition();
            StateManager = new StateManager(this);
            ActivitySchedule = new SimpleActivitySchedule();
        }

        public async Task Initialize()
        {
            // 1. Setup scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            Scheduler = await schedulerFactory.GetScheduler();

            await Scheduler.Start();

            // 2. Initialize StateManager, starting state execution
            StateManager.Initialize();
        }
    }
}

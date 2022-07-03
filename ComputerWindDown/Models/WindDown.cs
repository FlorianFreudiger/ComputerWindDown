using System.Diagnostics;
using ComputerWindDown.Models.Grayscale;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;
using Quartz;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly StateManager StateManager;
        public readonly GrayscaleTransition GrayscaleTransition;

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

        public WindDown()
        {
            StateManager = new StateManager(this);
            Coordinator coordinator = new Coordinator();
            GrayscaleTransition = new NvidiaDigitalVibranceTransition(coordinator);
        }
    }
}

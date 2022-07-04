using ComputerWindDown.Models.Grayscale;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;
using Quartz;
using System.Diagnostics;

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
            Coordinator coordinator = new Coordinator();
            GrayscaleTransition = new NvidiaDigitalVibranceTransition(coordinator);
            StateManager = new StateManager(this);
        }
    }
}

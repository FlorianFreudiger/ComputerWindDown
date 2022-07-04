using ComputerWindDown.Models.Grayscale;
using ComputerWindDown.Models.State;
using Quartz;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly StateManager StateManager;
        public readonly GrayscaleTransition GrayscaleTransition;

        public Subject<double> TransitionProgress;

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
            TransitionProgress = new Subject<double>();
            
            GrayscaleTransition = new NvidiaDigitalVibranceTransition(this);
            StateManager = new StateManager(this);
        }
    }
}

﻿using ComputerWindDown.Models.Grayscale;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading.Tasks;

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

        public IActivitySchedule ActivitySchedule;

        public WindDown()
        {
            TransitionProgress = new Subject<double>();
            
            GrayscaleTransition = new NvidiaDigitalVibranceTransition(this);
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

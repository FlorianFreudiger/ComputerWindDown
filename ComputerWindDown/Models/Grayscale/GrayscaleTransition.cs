using System;
using ComputerWindDown.Models.Time;

namespace ComputerWindDown.Models.Grayscale
{
    internal abstract class GrayscaleTransition
    {
        public GrayscaleTransition(Coordinator coordinator)
        {
            coordinator.TransitionProgress.Subscribe(d => SetTransition(d));
        }

        public abstract void SetTransition(double progress);
    }
}

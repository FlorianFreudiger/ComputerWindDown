using System;

namespace ComputerWindDown.Models.Grayscale
{
    internal abstract class GrayscaleTransition
    {
        public GrayscaleTransition(WindDown windDown)
        {
            windDown.TransitionProgress.Subscribe(d => SetTransition(d));
        }

        public abstract void SetTransition(double progress);
    }
}

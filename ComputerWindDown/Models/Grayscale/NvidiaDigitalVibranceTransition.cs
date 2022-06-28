using NvAPIWrapper.Display;

namespace ComputerWindDown.Models.Grayscale
{
    internal class NvidiaDigitalVibranceTransition : GrayscaleTransition
    {
        public NvidiaDigitalVibranceTransition()
        {
            NvAPIWrapper.NVIDIA.Initialize();
        }

        public override void SetTransition(double progress)
        {
            var displays = Display.GetDisplays();
            foreach (Display display in displays)
            {
                display.DigitalVibranceControl.NormalizedLevel = -progress;
            }
        }
    }
}

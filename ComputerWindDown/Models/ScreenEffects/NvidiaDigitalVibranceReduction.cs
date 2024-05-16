using NvAPIWrapper.Display;

namespace ComputerWindDown.Models.ScreenEffects;

internal class NvidiaDigitalVibranceReduction : ScreenEffect
{
    public NvidiaDigitalVibranceReduction()
    {
        NvAPIWrapper.NVIDIA.Initialize();
    }

    public override bool SupportsFractions()
    {
        return true;
    }

    public override void SetFraction(double fraction)
    {
        var displays = Display.GetDisplays();
        foreach (Display display in displays)
        {
            display.DigitalVibranceControl.NormalizedLevel = -fraction;
        }
    }
}

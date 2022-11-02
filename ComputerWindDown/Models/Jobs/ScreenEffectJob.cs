using System.Threading.Tasks;
using ComputerWindDown.Models.ScreenEffects;
using Quartz;

namespace ComputerWindDown.Models.Jobs;

// ReSharper disable once ClassNeverInstantiated.Global
// Job instantiated by Quartz

internal class ScreenEffectJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var jobDataMap = context.MergedJobDataMap;

        var screenEffectController = (ScreenEffectController)jobDataMap.Get(nameof(ScreenEffectController));
        bool effectEnabled = (bool)jobDataMap.Get(nameof(effectEnabled));

        screenEffectController.EffectState = effectEnabled;

        return Task.CompletedTask;
    }
}

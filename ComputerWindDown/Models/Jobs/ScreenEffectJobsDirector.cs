using System.Diagnostics;
using System.Threading.Tasks;
using ComputerWindDown.Models.ScreenEffects;
using ComputerWindDown.Properties;
using Quartz;

namespace ComputerWindDown.Models.Jobs;

internal class ScreenEffectJobsDirector
{
    private bool _isScheduled;

    private readonly DailyJobWrapper<ScreenEffectJob> _activationJobWrapper;
    private readonly DailyJobWrapper<ScreenEffectJob> _deactivationJobWrapper;

    public ScreenEffectJobsDirector(ScreenEffectController screenEffectController)
    {
        _activationJobWrapper = new DailyJobWrapper<ScreenEffectJob>(new JobDataMap
        {
            { nameof(ScreenEffectController), screenEffectController },
            { "effectEnabled", true }
        });

        Debug.Assert(screenEffectController != null);
        _deactivationJobWrapper = new DailyJobWrapper<ScreenEffectJob>(new JobDataMap
        {
            { nameof(ScreenEffectController), screenEffectController },
            { "effectEnabled", false }
        });
    }

    public async Task Start()
    {
        if (_isScheduled)
        {
            return;
        }

        await _activationJobWrapper.SetTimeOfDay(Settings.Default.StartTime);
        await _deactivationJobWrapper.SetTimeOfDay(Settings.Default.EndTime);

        await _activationJobWrapper.Start();
        await _deactivationJobWrapper.Start();

        // Run one of the jobs immediately to set initial state
        if (IsCurrentlyActive())
        {
            await _activationJobWrapper.TriggerNow();
        }
        else
        {
            await _deactivationJobWrapper.TriggerNow();
        }

        _isScheduled = true;
    }

    public async Task Stop()
    {
        if (!_isScheduled)
        {
            return;
        }

        await _activationJobWrapper.Stop();
        await _deactivationJobWrapper.Stop();

        _isScheduled = false;
    }

    public async Task Restart()
    {
        await Stop();
        await Start();
    }

    public bool IsCurrentlyActive()
    {
        Debug.Assert(_activationJobWrapper.Trigger != null);
        Debug.Assert(_deactivationJobWrapper.Trigger != null);

        return _activationJobWrapper.Trigger.GetNextFireTimeUtc() > _deactivationJobWrapper.Trigger.GetNextFireTimeUtc();
    }
}

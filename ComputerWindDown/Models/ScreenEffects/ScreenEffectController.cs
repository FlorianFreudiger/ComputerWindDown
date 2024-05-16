using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.ScreenEffects;

internal class ScreenEffectController
{
    // TODO: Expose transition duration through user preferences
    private static readonly TimeSpan TransitionTickTime = TimeSpan.FromMilliseconds(100);
    private const double TransitionTickDelta = 0.01;

    public readonly ISet<ScreenEffect> AllScreenEffects;

    private volatile bool _transitionActive;

    private ScreenEffect _currentScreenEffect;

    public ScreenEffect CurrentScreenEffect
    {
        get => _currentScreenEffect;
        set
        {
            if (_currentScreenEffect == value) return;

            ScreenEffect oldEffect = _currentScreenEffect;
            _currentScreenEffect = value;

            // Stop transition if active
            _transitionActive = false;

            if (EffectState)
            {
                oldEffect.SetActive(false);
                _currentScreenEffect.SetActive(true);
            }
        }
    }

    private bool _effectState;

    public bool EffectState
    {
        get => _effectState;
        set
        {
            if (_effectState == value) return;
            _effectState = value;

            if (CurrentScreenEffect.SupportsFractions())
            {
                if (!_transitionActive)
                {
                    _transitionActive = true;
                    // Start asynchronous transition
                    _ = Transition();
                }
            }
            else
            {
                _currentScreenEffect.SetActive(_effectState);
            }
        }
    }

    public ScreenEffectController()
    {
        _effectState = false;

        ScreenEffect nvidiaScreenEffect = new NvidiaDigitalVibranceReduction();

        AllScreenEffects = new HashSet<ScreenEffect>
        {
            nvidiaScreenEffect
        };

        // TODO: After adding other screen effects, pick effect based on user preferences
        _currentScreenEffect = nvidiaScreenEffect;
    }

    public void Reset()
    {
        // Set effectState directly (instead of through property), skipping transition
        _effectState = false;
        CurrentScreenEffect.SetActive(false);
        _transitionActive = false;
    }

    private async Task Transition()
    {
        using PeriodicTimer periodic = new PeriodicTimer(TransitionTickTime);
        double fraction = EffectState ? 0 : 1;
        while (await periodic.WaitForNextTickAsync())
        {
            if (!_transitionActive || (EffectState && fraction >= 1.0) || (!EffectState && fraction <= 0.0))
            {
                _transitionActive = false;
                break;
            }

            double delta = EffectState ? TransitionTickDelta : -TransitionTickDelta;
            fraction += delta;
            CurrentScreenEffect.SetFraction(fraction);
        }
    }
}

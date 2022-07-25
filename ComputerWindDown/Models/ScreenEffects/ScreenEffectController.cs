using System;
using System.Collections.Generic;
using System.Threading;

namespace ComputerWindDown.Models.ScreenEffects
{
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
                if (EffectState)
                {
                    SetState(false, true);
                }

                _currentScreenEffect = value;

                if (EffectState)
                {
                    SetState(true, true);
                }
            }
        }

        public bool EffectState { get; private set; }

        public ScreenEffectController()
        {
            ScreenEffect nvidiaScreenEffect = new NvidiaDigitalVibranceReduction();

            AllScreenEffects = new HashSet<ScreenEffect>
            {
                nvidiaScreenEffect
            };

            // TODO: After adding other screen effects, pick effect based on user preferences
            _currentScreenEffect = nvidiaScreenEffect;
        }

        public void SetState(bool newState, bool skipTransition = false)
        {
            if (EffectState == newState) return;
            EffectState = newState;

            if (!skipTransition && CurrentScreenEffect.SupportsFractions())
            {
                if (!_transitionActive)
                {
                    _transitionActive = true;
                    Transition();
                }
            }
            else
            {
                _currentScreenEffect.SetActive(newState);
            }
        }

        private async void Transition()
        {
            PeriodicTimer periodic = new PeriodicTimer(TransitionTickTime);
            double fraction = EffectState ? 0 : 1;
            while (await periodic.WaitForNextTickAsync())
            {
                if ((EffectState && fraction >= 1.0) || (!EffectState && fraction <= 0.0))
                {
                    periodic.Dispose();
                    _transitionActive = false;
                    break;
                }

                double delta = EffectState ? TransitionTickDelta : -TransitionTickDelta;
                fraction += delta;
                CurrentScreenEffect.SetFraction(fraction);
            }
        }
    }
}

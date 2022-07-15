using System;

namespace ComputerWindDown.Models.ScreenEffects
{
    internal abstract class ScreenEffect
    {
        public virtual bool IsAvailable()
        {
            return true;
        }

        public virtual void SetActive(bool active)
        {
            if (SupportsFractions())
            {
                SetFraction(active ? 1.0 : 0.0);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public virtual bool SupportsFractions()
        {
            return false;
        }

        public virtual void SetFraction(double fraction)
        {
            if (SupportsFractions())
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}

using ComputerWindDown.Models.ScreenEffects;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;
using System.Threading.Tasks;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly StateManager StateManager;
        public readonly ScreenEffect ScreenEffect;

        public IActivitySchedule ActivitySchedule;

        public WindDown()
        {
            ScreenEffect = new NvidiaDigitalVibranceReduction();
            StateManager = new StateManager(this);
            ActivitySchedule = new SimpleActivitySchedule();
        }

        public async Task Initialize()
        {
            // Initialize StateManager, starting state execution
            await StateManager.Initialize();
        }
    }
}

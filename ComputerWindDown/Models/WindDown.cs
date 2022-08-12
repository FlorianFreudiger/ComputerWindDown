using System.Threading.Tasks;
using ComputerWindDown.Models.ScreenEffects;
using ComputerWindDown.Models.State;
using ComputerWindDown.Models.Time;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly StateManager StateManager;
        public readonly ScreenEffectController ScreenEffectController;

        public IActivitySchedule ActivitySchedule;

        public WindDown()
        {
            ScreenEffectController = new ScreenEffectController();
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

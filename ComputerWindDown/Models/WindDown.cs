using System.ComponentModel;
using System.Threading.Tasks;
using ComputerWindDown.Models.Jobs;
using ComputerWindDown.Models.ScreenEffects;
using ComputerWindDown.Properties;
using Quartz.Impl;

namespace ComputerWindDown.Models
{
    internal class WindDown
    {
        public readonly ScreenEffectController ScreenEffectController;
        public readonly ScreenEffectJobsDirector ScreenEffectJobsDirector;

        public WindDown()
        {
            ScreenEffectController = new ScreenEffectController();
            ScreenEffectJobsDirector = new ScreenEffectJobsDirector(ScreenEffectController);

            Settings.Default.PropertyChanged += SettingsChanged;
        }

        private void SettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            _ = Update();
        }

        public async Task Initialize()
        {
            // Start the default scheduler (if not started already)
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            await Update();
        }

        private async Task Update()
        {
            if (Settings.Default.Enable)
            {
                await ScreenEffectJobsDirector.Restart();
            }
            else
            {
                await ScreenEffectJobsDirector.Stop();
                ScreenEffectController.Reset();
            }
        }
    }
}

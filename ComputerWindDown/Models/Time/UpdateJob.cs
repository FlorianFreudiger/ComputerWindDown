using Quartz;
using System.Threading.Tasks;

namespace ComputerWindDown.Models.Time
{
    internal class UpdateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}

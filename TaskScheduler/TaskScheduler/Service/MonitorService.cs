using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace TaskScheduler.Service
{
    public class MonitorService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await AddJob();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task AddJob()
        {

            BackgroundJob.Schedule(() => Print("Agendamento", null), TimeSpan.FromSeconds(5));

            var jobID = BackgroundJob.Enqueue("testeequeue", () => Print("Test in Queue", null));

            BackgroundJob.ContinueJobWith(jobID, () => Print($"performed after finishing the Job Id: {jobID}", null));

            RecurringJob.AddOrUpdate("RecurringJob", () => Print("Recurring", null), Cron.MinuteInterval(1));

        }

        public string Print(string message, PerformContext? context)
        {
            context?.WriteLine(message);
            return $"Printed: {message}";
        }

        public string PrintRecurringJob(string message, PerformContext? context)
        {
            context?.WriteLine(message);
            return $"Printed: {message}";
        }

    }
}
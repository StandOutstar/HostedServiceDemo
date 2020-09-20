using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
// using Timer = System.Timers.Timer;
using Timer = System.Threading.Timer;

namespace HostedServiceDemo.HostedServices.CronJobs
{
    public abstract class CronJobService: IHostedService, IDisposable
    {
        private Timer _timer;
        private int _executionCount;
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _cronExpression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var delay = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            _timer = new Timer(state =>  DoWork(state).GetAwaiter().GetResult(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            Console.WriteLine(
                $"{DateTime.Now:hh:mm:ss.fff} Timed Hosted Service ({this}) is working. Count: {count}");
            await Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // _timer?.Stop();
            _timer?.Change(Timeout.Infinite, 0);
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;
//using Timer = System.Threading.Timer;

namespace HostedServiceDemo.HostedServices.CronJobs
{
    public abstract class CronJobService: IHostedService, IDisposable
    {
        private Timer _timer;
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
            var next = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into timer
                {
                    Console.WriteLine($"{this} at: {DateTimeOffset.Now} delay: {delay}");
                    await ScheduleJob(cancellationToken);
                }
                else
                {
                    Console.WriteLine($"schedule next {this} job delay: {delay.TotalMilliseconds}");
                    _timer = new Timer(delay.TotalMilliseconds);
                    _timer.Elapsed += async (sender, args) =>
                    {
                        _timer.Dispose();  // reset and dispose timer
                         _timer = null;

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await DoWork(cancellationToken);
                        }

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await ScheduleJob(cancellationToken);    // reschedule next
                        }
                    };
                    _timer.Start();
                }
            }
            await Task.CompletedTask;
        }

        
        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            //_timer?.Change(Timeout.Infinite, 0);
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
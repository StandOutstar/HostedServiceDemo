using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HostedServiceDemo.HostedServices.CronJobs
{
    public class MyCronJob2: CronJobService
    {
        private readonly ILogger<MyCronJob2> _logger;

        public MyCronJob2(IScheduleConfig<MyCronJob2> scheduleConfig, ILogger<MyCronJob2> logger) : base(scheduleConfig.CronExpresion, scheduleConfig.TimeZoneInfo)
        {
            _logger = logger;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 starts.");
            return base.StartAsync(cancellationToken);
        }
        
        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss.fff t z} CronJob 2 is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 is stopping.");
            return base.StopAsync(cancellationToken);
        }


    }
}
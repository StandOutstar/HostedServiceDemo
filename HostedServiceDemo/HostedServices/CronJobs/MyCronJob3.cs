using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HostedServiceDemo.HostedServices.CronJobs
{
    public class MyCronJob3: CronJobService
    {
        private readonly ILogger<MyCronJob3> _logger;

        public MyCronJob3(IScheduleConfig<MyCronJob3> scheduleConfig, ILogger<MyCronJob3> logger) : base(scheduleConfig.CronExpresion, scheduleConfig.TimeZoneInfo)
        {
            _logger = logger;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 starts.");
            return base.StartAsync(cancellationToken);
        }
        
        public override Task DoWork(object state)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss.fff t z} CronJob 3 is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 is stopping.");
            return base.StopAsync(cancellationToken);
        }


    }
}
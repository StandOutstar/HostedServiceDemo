using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HostedServiceDemo.HostedServices.CronJobs
{
    public class MyCronJob1: CronJobService
    {
        private readonly ILogger<MyCronJob1> _logger;

        public MyCronJob1(IScheduleConfig<MyCronJob1> scheduleConfig, ILogger<MyCronJob1> logger) : base(scheduleConfig.CronExpresion, scheduleConfig.TimeZoneInfo)
        {
            _logger = logger;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }
        
        public override async Task DoWork(object state)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss.fff t z} CronJob 1 is working.");
            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }


    }
}
using System;
using Cronos;
using HostedServiceDemo.HostedServices;
using HostedServiceDemo.HostedServices.CronJobs;
using Microsoft.Extensions.DependencyInjection;

namespace HostedServiceDemo.Extensions
{
    public static class ScheduledServiceExtensions
    {
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services,
            Action<IScheduleConfig<T>> options) where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configuration.");
            }

            var config = new ScheduleConfig<T>();
            
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpresion))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpresion), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();

            return services;
        }
    }
}
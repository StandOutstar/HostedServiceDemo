using System;

namespace HostedServiceDemo.HostedServices
{
    public interface IScheduleConfig<T>
    {
        string CronExpresion { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
    public class ScheduleConfig<T>: IScheduleConfig<T>
    {
        public string CronExpresion { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
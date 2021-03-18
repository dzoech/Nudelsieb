using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.Notifications
{
    public class NotificationsOptions
    {
        public const string SectionName = "Notifications";
        public AzureNotificationHubOptions AzureNotificationHub { get; set; } = new AzureNotificationHubOptions();
        public SchedulerOptions Scheduler { get; set; } = new SchedulerOptions();
    }

    public class AzureNotificationHubOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
    }

    public class SchedulerOptions
    {
        public AzureServiceBusOptions AzureServiceBus { get; set; } = new AzureServiceBusOptions();
    }

    public class AzureServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;

    }
}

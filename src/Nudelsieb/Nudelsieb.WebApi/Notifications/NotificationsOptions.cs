using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Notifications
{
    public class NotificationsOptions
    {
        public const string SectionName = "Notifications";

        public string BindTest { get; set; } = string.Empty;

        public AzureNotificationHubOptions AzureNotificationHub { get; set; } = new();

        public SchedulerOptions Scheduler { get; set; } = new();
    }

    public class AzureNotificationHubOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
    }

    public class SchedulerOptions
    {
        public AzureServiceBusOptions AzureServiceBus { get; set; } = new();
    }

    public class AzureServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;

    }
}

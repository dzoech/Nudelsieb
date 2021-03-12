using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace Nudelsieb.Notifications.Scheduler
{
    public class ServiceBusNotificationScheduler : INotificationScheduler
    {
        private readonly ServiceBusClient serviceBusClient;
        private readonly IOptions<NotificationsOptions> notificationsOptions;
        private readonly string queueName;

        public ServiceBusNotificationScheduler(ServiceBusClient serviceBusClient, IOptions<NotificationsOptions> notificationsOptions)
        {
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
            this.notificationsOptions = notificationsOptions ?? throw new ArgumentNullException(nameof(notificationsOptions));
            queueName = notificationsOptions.Value.Scheduler.AzureServiceBus.QueueName;

            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException($"Queue name '{queueName}' is not allowed");
        }

        public async Task ScheduleAsync(string notification, DateTimeOffset schedulingTime)
        {
            // TODO: use sender as singleton 
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-performance-improvements?tabs=net-standard-sdk-2#reusing-factories-and-clients
                       
            await using var sender = serviceBusClient.CreateSender(queueName);

            var message = new ServiceBusMessage(notification)
            {
                MessageId = Guid.NewGuid().ToString()
            };

            await sender.ScheduleMessageAsync(message, schedulingTime);
        }
    }
}

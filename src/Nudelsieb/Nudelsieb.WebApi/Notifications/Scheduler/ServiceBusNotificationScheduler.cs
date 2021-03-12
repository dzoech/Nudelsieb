using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Nudelsieb.WebApi.Notifications.Scheduler
{
    public class ServiceBusNotificationScheduler : INotificationScheduler
    {
        private readonly ServiceBusClient serviceBusClient;
        private readonly string queueName = "reminder-push-notifications";

        public ServiceBusNotificationScheduler(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
        }

        public async Task ScheduleAsync(string notification, DateTimeOffset schedulingTime)
        {
            // TODO: use sender as singleton https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-performance-improvements?tabs=net-standard-sdk-2#reusing-factories-and-clients
            await using var sender = serviceBusClient.CreateSender(queueName);

            var message = new ServiceBusMessage(notification)
            {
                MessageId = Guid.NewGuid().ToString()
            };

            await sender.ScheduleMessageAsync(message, schedulingTime);
        }
    }
}

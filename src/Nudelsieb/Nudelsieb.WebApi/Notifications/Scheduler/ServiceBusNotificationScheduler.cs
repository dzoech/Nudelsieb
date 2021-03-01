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
        private readonly string queueName = "push-notifications";

        public ServiceBusNotificationScheduler(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
        }

        public async Task ScheduleAsync(string notification, DateTimeOffset schedulingTime)
        {
            await using var sender = serviceBusClient.CreateSender(queueName);

            var message = new ServiceBusMessage(notification)
            {
                MessageId = Guid.NewGuid().ToString()
            };

            await sender.ScheduleMessageAsync(message, schedulingTime);
        }
    }
}

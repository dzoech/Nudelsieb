using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nudelsieb.Application.Notifications;

namespace Nudelsieb.Notifications.Scheduler
{
    public class ServiceBusNotificationScheduler : INotificationScheduler
    {
        private readonly ServiceBusClient serviceBusClient;
        private readonly IOptions<NotificationsOptions> notificationsOptions;
        private readonly string queueName;
        private readonly ILogger<ServiceBusNotificationScheduler> logger;

        public ServiceBusNotificationScheduler(
            ServiceBusClient serviceBusClient,
            IOptions<NotificationsOptions> notificationsOptions,
            ILogger<ServiceBusNotificationScheduler> logger)
        {
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
            this.notificationsOptions = notificationsOptions ?? throw new ArgumentNullException(nameof(notificationsOptions));
            queueName = notificationsOptions.Value.Scheduler.AzureServiceBus.QueueName;

            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException($"Queue name '{queueName}' is not allowed");
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ScheduleAsync(string notification, DateTimeOffset schedulingTime)
        {
            logger.LogInformation(
                "Scheduling notification '{notification}' at {schedulingTime}",
                notification,
                schedulingTime);

            logger.LogInformation("Creating sender for queue '{queueName}'...", queueName);

            // TODO: use sender as singleton
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-performance-improvements?tabs=net-standard-sdk-2#reusing-factories-and-clients

            await using var sender = serviceBusClient.CreateSender(queueName);

            var message = new ServiceBusMessage(notification)
            {
                MessageId = Guid.NewGuid().ToString()
            };

            logger.LogInformation("Assigned id '{guid}' to Service Bus message.", message.MessageId);

            await sender.ScheduleMessageAsync(message, schedulingTime);
        }
    }
}

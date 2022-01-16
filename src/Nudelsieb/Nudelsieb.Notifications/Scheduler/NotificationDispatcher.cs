using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nudelsieb.Application.Notifications;

namespace Nudelsieb.Notifications.Scheduler
{
    public class NotificationDispatcher : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ServiceBusClient serviceBusClient;
        private readonly IOptions<NotificationsOptions> notificationsOptions;
        private readonly ILogger<NotificationDispatcher> logger;
        private readonly ServiceBusProcessor serviceBusProcessor;
        private readonly string queueName;

        public NotificationDispatcher(
            IServiceScopeFactory serviceScopeFactory,
            ServiceBusClient serviceBusClient,
            IOptions<NotificationsOptions> notificationsOptions,
            ILogger<NotificationDispatcher> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
            this.notificationsOptions = notificationsOptions ?? throw new ArgumentNullException(nameof(notificationsOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            queueName = notificationsOptions.Value.Scheduler.AzureServiceBus.QueueName;

            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException($"Queue name '{queueName}' is not allowed");

            logger.LogInformation(
                "Creating Service Bus processor for queue '{queueName}' in '{namespace}'.",
                queueName,
                serviceBusClient.FullyQualifiedNamespace);

            serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);
            serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync;
            serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start processing...");
            await serviceBusProcessor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stop processing...");
            await serviceBusProcessor.StopProcessingAsync(cancellationToken);
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            logger.LogInformation("Processing message received by Service Bus");

            // Services with scoped lifetime cannot be injected directly into a HostedService
            using var scope = serviceScopeFactory.CreateScope();
            var pushNotifyer = scope.ServiceProvider.GetRequiredService<IPushNotifyer>();
            var message = args.Message.Body.ToString();
            await pushNotifyer.SendAsync(message, "ANY");
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogWarning(args.Exception, "Received an error by Service Bus");

            return Task.CompletedTask;
        }
    }
}

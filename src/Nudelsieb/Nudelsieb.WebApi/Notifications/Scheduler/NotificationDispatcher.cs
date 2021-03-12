using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nudelsieb.WebApi.Notifications.Notifyer;

namespace Nudelsieb.WebApi.Notifications.Scheduler
{
    public class NotificationDispatcher : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ServiceBusClient serviceBusClient;
        private readonly ILogger<NotificationDispatcher> logger;
        private readonly ServiceBusProcessor serviceBusProcessor;
        private readonly string queueName = "reminder-push-notifications";


        public NotificationDispatcher(
            IServiceScopeFactory serviceScopeFactory,
            ServiceBusClient serviceBusClient,
            ILogger<NotificationDispatcher> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);
            serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync;
            serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            logger.LogDebug("Processing message received by Service Bus");

            // Services with scoped lifetime cannot be injected directly into a HostedService
            using var scope = serviceScopeFactory.CreateScope();
            var pushNotifyer = scope.ServiceProvider.GetRequiredService<IPushNotifyer>();
            await pushNotifyer.SendAsync(args.Message.Body.ToString(), "ANY");
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogWarning(args.Exception, "Received an error by Service Bus");

            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await serviceBusProcessor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await serviceBusProcessor.StopProcessingAsync(cancellationToken);
        }
    }
}

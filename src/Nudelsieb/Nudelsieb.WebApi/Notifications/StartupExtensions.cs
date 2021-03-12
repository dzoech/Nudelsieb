using System;
using System.Net.Http;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nudelsieb.WebApi.Notifications.Notifyer;
using Nudelsieb.WebApi.Notifications.Scheduler;

namespace Nudelsieb.WebApi.Notifications
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddNotificationServices(
            this IServiceCollection services,
            Action<NotificationsOptions> configureOptions)
        {
            // register the configured options for DI
            services.AddOptions<NotificationsOptions>().Configure(o => configureOptions(o));
            services.AddScoped<IPushNotifyer, AndroidNotifyer>();

            services.AddScoped<INotificationHubClient>(provider =>
            {
                var factory = provider.GetRequiredService<IHttpMessageHandlerFactory>();
                var options = provider.GetRequiredService<IOptions<NotificationsOptions>>();

                var hubSettings = new NotificationHubSettings
                {
                    MessageHandler = factory.CreateHandler()
                };

                return new NotificationHubClient(
                    options.Value.AzureNotificationHub.ConnectionString,
                    options.Value.AzureNotificationHub.HubName,
                    hubSettings);
            });

            // docs: https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/servicebus/Azure.Messaging.ServiceBus
            services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<NotificationsOptions>>();
                return new ServiceBusClient(options.Value.Scheduler.AzureServiceBus.ConnectionString);
            });
            services.AddScoped<INotificationScheduler, ServiceBusNotificationScheduler>();
            services.AddHostedService<NotificationDispatcher>();

            return services;
        }
    }
}

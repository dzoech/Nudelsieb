using System;
using System.Net.Http;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.DependencyInjection;
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
            var options = new NotificationsOptions();
            configureOptions(options);

            // register the configured options for DI
            services.AddOptions<NotificationsOptions>().Configure(o => o = options);

            services.AddScoped<IPushNotifyer, AndroidNotifyer>();

            services.AddScoped<INotificationHubClient>(provider =>
            {
                var factory = provider.GetRequiredService<IHttpMessageHandlerFactory>();

                var hubSettings = new NotificationHubSettings
                {
                    MessageHandler = factory.CreateHandler()
                };

                return new NotificationHubClient(
                    options.AzureNotificationHub.ConnectionString,
                    options.AzureNotificationHub.HubName,
                    hubSettings);
            });

            services.AddSingleton(new ServiceBusClient(options.Scheduler.AzureServiceBus.ConnectionString));
            services.AddScoped<INotificationScheduler, ServiceBusNotificationScheduler>();

            return services;
        }
    }
}

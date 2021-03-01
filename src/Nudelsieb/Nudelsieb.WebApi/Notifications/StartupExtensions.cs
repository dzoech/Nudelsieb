using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Nudelsieb.WebApi.Notifications.Notifyer;

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

            services.AddSingleton(new ServiceBusClient(options.Scheduler.AzureServiceBus.ConnectionString));
            services.AddScoped<IPushNotifyer, AndroidNotifyer>();

            return services;
        }
    }
}

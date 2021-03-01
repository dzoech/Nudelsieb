using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton(new ServiceBusClient(options.Scheduler.AzureServiceBus.ConnectionString));

            return services;
        }
    }
}

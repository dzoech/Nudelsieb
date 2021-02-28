using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nudelsieb.WebApi.Notifications
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            return services.AddScoped<IPushNotifyer, AndroidNotifyer>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Nudelsieb.Application.UseCases;

namespace Nudelsieb.Application
{
    public static class DependecyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<ISetReminderUseCase, SetReminderUseCase>();

            return services;
        }
    }
}

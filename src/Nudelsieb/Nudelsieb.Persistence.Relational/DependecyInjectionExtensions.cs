using Microsoft.Extensions.DependencyInjection;
using Nudelsieb.Domain.Aggregates;
using Nudelsieb.Persistence.Relational.Repositories;

namespace Nudelsieb.Persistence.Relational
{
    public static class DependecyInjectionExtensions
    {
        public static IServiceCollection AddRelationalPersistence(this IServiceCollection services)
        {
            services.AddTransient<INeuronRepository, NeuronRepository>();
            services.AddTransient<IReminderRepository, ReminderRepository>();

            return services;
        }
    }
}

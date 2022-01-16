using Microsoft.Extensions.DependencyInjection;
using Nudelsieb.Application.UseCases;

namespace Nudelsieb.Application
{
    public static class DependecyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<SetRemindersUseCase>();
            services.AddScoped<NeurogenesisUseCase>();
            services.AddScoped<GetEverythingUseCase>();

            return services;
        }
    }
}

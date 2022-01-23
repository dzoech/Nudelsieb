using Microsoft.Extensions.DependencyInjection;

namespace Nudelsieb.WebApi
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services
                .AddHttpContextAccessor() // required by UserService
                .AddScoped<UserService>();
            return services;
        }
    }
}

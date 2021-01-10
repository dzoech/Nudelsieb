using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.Services;
using Refit;
using System;
using Nudelsieb.Shared.Clients.Authentication;

namespace Nudelsieb.Cli.RestClients
{
    static class RestClientsExtensions
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services)
        {
            services.AddTransient<IBraindumpRestClient>(sp =>
            {
                var endpointsOptions = sp.GetRequiredService<IOptions<EndpointsOptions>>();

                return RestService.For<IBraindumpRestClient>(
                    endpointsOptions.Value.Braindump?.Value ?? throw new ArgumentNullException(),
                    new RefitSettings
                    {
                        AuthorizationHeaderValueGetter = async () =>
                        {
                            var authService = sp.GetRequiredService<IAuthenticationService>();
                            (var success, var token) = await authService.GetCachedAccessTokenAsync();

                            if (success)
                            {
                                return token!.RawData;
                            }

                            return string.Empty;
                        }
                    });
            });

            return services;
        }
    }
}

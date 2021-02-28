using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nudelsieb.Cli.Options;
using Refit;
using System;
using Nudelsieb.Shared.Clients.Authentication;
using System.Threading.Tasks;

namespace Nudelsieb.Cli.RestClients
{
    static class RestClientsExtensions
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services)
        {
            services.AddTransient<IBraindumpRestClient>(sp =>
            {
                Func<Task<string>> accessTokenRetriever = async () =>
                {
                    var authService = sp.GetRequiredService<IAuthenticationService>();
                    (var success, var accessToken) = await authService.GetCachedAccessTokenAsync();

                    if (success)
                    {
                        return accessToken!.RawData;
                    }

                    return string.Empty;
                };

                var endpointsOptions = sp.GetRequiredService<IOptions<EndpointsOptions>>();

                return RestService.For<IBraindumpRestClient>(
                    endpointsOptions.Value.Braindump?.Value ?? throw new ArgumentNullException(),
                    new RefitSettings
                    {
                        AuthorizationHeaderValueGetter = accessTokenRetriever
                    });
            });

            return services;
        }
    }
}

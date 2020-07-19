using Microsoft.Extensions.DependencyInjection;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.RestClients
{
    static class RestClientsExtensions
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services, EndpointsOptions endpointsOptions)
        {
            services.AddTransient<IBraindumpRestClient>(sp =>
                RestService.For<IBraindumpRestClient>(
                    endpointsOptions.Braindump ?? throw new ArgumentNullException(),
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
                    }));

            return services;
        }
    }
}

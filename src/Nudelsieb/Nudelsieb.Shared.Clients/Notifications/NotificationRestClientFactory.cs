using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nudelsieb.Shared.Clients.Authentication;
using Refit;

namespace Nudelsieb.Shared.Clients.Notifications
{
    public class NotificationRestClientFactory
    {
        private readonly IAuthenticationService authService;
        private readonly ILogger<NotificationRestClientFactory> logger;

        public NotificationRestClientFactory(
            IAuthenticationService authService,
            ILogger<NotificationRestClientFactory> logger)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public INotificationsRestClient Create(Uri endpoint)
        {
            var client = RestService.For<INotificationsRestClient>(
                endpoint.ToString(),
                //"https://nudelsieb.zoechbauer.dev/braindump/",
                //endpointsOptions.Value.Notifications?.Value ?? throw new ArgumentNullException(),
                new RefitSettings
                {
                    AuthorizationHeaderValueGetter = async () =>
                    {
                        (var success, var token) = await authService.GetCachedAccessTokenAsync();

                        if (success)
                        {
                            return token!.RawData;
                        }
                        else
                        {
                            logger.LogError("Could not get cached access token");
                            return string.Empty;
                        }
                    }
                });

            return client;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Nudelsieb.Shared.Clients.Authentication;
using Refit;

namespace Nudelsieb.Shared.Clients.Notifications
{
    public class AuthorizedRestClientFactory
    {
        private readonly IAuthenticationService authService;
        private readonly ILogger<AuthorizedRestClientFactory> logger;

        public AuthorizedRestClientFactory(
            IAuthenticationService authService,
            ILogger<AuthorizedRestClientFactory> logger)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public TRestClientInterface Create<TRestClientInterface>(Uri endpoint, bool allowSelfSignedCertificates = false)
            where TRestClientInterface : class
        {
            var settings = new RefitSettings
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
            };

            if (allowSelfSignedCertificates)
            {
                EnableSelfSignedCertificates(settings);
            }

            var client = RestService.For<TRestClientInterface>(endpoint.ToString(), settings);

            return client;
        }

        private void EnableSelfSignedCertificates(RefitSettings settings)
        {
            var selfSignedCertHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) => true
            };

            settings.HttpMessageHandlerFactory = () => selfSignedCertHandler;
        }
    }
}

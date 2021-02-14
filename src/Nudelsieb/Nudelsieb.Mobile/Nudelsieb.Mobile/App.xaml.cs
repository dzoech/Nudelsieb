using System;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Configuration;
using Nudelsieb.Mobile.RestClients;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Utils;
using Nudelsieb.Mobile.Views;
using Nudelsieb.Shared.Clients;
using Nudelsieb.Shared.Clients.Authentication;
using Nudelsieb.Shared.Clients.Notifications;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile
{
    public partial class App : Application
    {
        public static IAuthenticationService AuthenticationService { get; private set; }
        public static IBraindumpRestClient BraindumpRestClient { get; set; }
        public static INotificationsRestClient NotificationsRestClient { get; set; }

        public static object UiParent { get; set; } = null;

        public App()
        {
            InitializeComponent();

            var authenticationClient =
                PublicClientApplicationBuilder.Create(AppSettings.Settings.Auth.ClientId)
                    .WithParentActivityOrWindow(() => App.UiParent)
                    .WithB2CAuthority(AppSettings.Settings.Auth.AuthoritySignUpSignin)
                    .WithRedirectUri($"msal{AppSettings.Settings.Auth.ClientId}://auth")
                    .Build();

            AuthOptions auth = new AuthOptions
            {
                ClientId = AppSettings.Settings.Auth.ClientId,
                RequiredScopes = AppSettings.Settings.Auth.RequiredScopes,
                PolicySignUpSignIn = AppSettings.Settings.Auth.PolicySignUpSignIn
                // todo map, or better consolidate
            };

            var logger = new DebugLogger<AuthenticationService>();
            AuthenticationService = new AuthenticationService(logger, authenticationClient, new SimpleOptions<AuthOptions>(auth));

            var restClientFactory = new AuthorizedRestClientFactory(
                AuthenticationService,
                new DebugLogger<AuthorizedRestClientFactory>());

            var braindumpUri = new Uri("https://192.168.8.201:5001/braindump/");
            BraindumpRestClient = restClientFactory.Create<IBraindumpRestClient>(
                braindumpUri, // endpointsOptions.Value.Braindump?.Value
                allowSelfSignedCertificates: true);

            
            var notificationsUri = new Uri("https://192.168.8.201:5001/notifications/");
            NotificationsRestClient = restClientFactory.Create<INotificationsRestClient>(
                notificationsUri,
                allowSelfSignedCertificates: true);

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

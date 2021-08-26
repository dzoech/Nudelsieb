using System;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Configuration;
using Nudelsieb.Mobile.RestClients;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Utils;
using Nudelsieb.Mobile.ViewModels;
using Nudelsieb.Shared.Clients.Authentication;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Forms;

[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "Montserrat-Bold")]
[assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
[assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
[assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]

namespace Nudelsieb.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                AppSettings.Settings.SyncfusionLicenseKey);

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

            var braindumpUri = new Uri(AppSettings.Settings.Endpoints.Braindump.Value);
            BraindumpRestClient = restClientFactory.Create<IBraindumpRestClient>(
                braindumpUri,
                allowSelfSignedCertificates: true);

            var notificationsUri = new Uri(AppSettings.Settings.Endpoints.Notifications.Value);
            NotificationsRestClient = restClientFactory.Create<INotificationsRestClient>(
                notificationsUri,
                allowSelfSignedCertificates: true);

            DependencyService.Register<LoadingViewModel>();
            DependencyService.Register<MockDataStore>();

            MainPage = new AppShell();
        }

        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public static IAuthenticationService AuthenticationService { get; private set; }
        public static IBraindumpRestClient BraindumpRestClient { get; set; }
        public static INotificationsRestClient NotificationsRestClient { get; set; }

        public static object UiParent { get; set; } = null;
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

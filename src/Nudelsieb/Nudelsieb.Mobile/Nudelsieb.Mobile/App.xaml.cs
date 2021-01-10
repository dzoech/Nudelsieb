using System;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Configuration;
using Nudelsieb.Mobile.RestClients;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Views;
using Nudelsieb.Shared.Clients;
using Nudelsieb.Shared.Clients.Authentication;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile
{
    public partial class App : Application
    {
        public static IPublicClientApplication AuthenticationClient { get; private set; }
        public static IAuthenticationService AuthenticationService { get; private set; }
        public static IBraindumpRestClient BraindumpRestClient { get; set; }

        public static object UiParent { get; set; } = null;

        public App()
        {
            InitializeComponent();

            AuthenticationClient =
                PublicClientApplicationBuilder.Create(AppSettings.Settings.ClientId)
                    .WithIosKeychainSecurityGroup(AppSettings.Settings.IosKeychainSecurityGroups)
                    .WithB2CAuthority(AppSettings.Settings.AuthoritySignin)
                    .WithRedirectUri($"msal{AppSettings.Settings.ClientId}://auth")
                    .Build();

            AuthOptions auth = new AuthOptions
            {
                ClientId = AppSettings.Settings.ClientId
                // to map, or better consolidate
            };

            AuthenticationService = new AuthenticationService(null, AuthenticationClient, new SimpleOptions<AuthOptions>(auth));

            BraindumpRestClient = RestService.For<IBraindumpRestClient>(
                "https://nudelsieb.zoechbauer.dev",
                //endpointsOptions.Value.Braindump?.Value ?? throw new ArgumentNullException(),
                new RefitSettings
                {
                    AuthorizationHeaderValueGetter = async () =>
                    {
                        (var success, var token) = await AuthenticationService.GetCachedAccessTokenAsync();

                        if (success)
                        {
                            return token!.RawData;
                        }

                        return string.Empty;
                    }
                });

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

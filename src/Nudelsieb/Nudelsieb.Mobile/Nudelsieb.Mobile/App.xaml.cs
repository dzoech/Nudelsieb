using System;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile
{
    public partial class App : Application
    {
        public static IPublicClientApplication AuthenticationClient { get; private set; }

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

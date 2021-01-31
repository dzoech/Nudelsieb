﻿using System;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Configuration;
using Nudelsieb.Mobile.RestClients;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Utils;
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
                PublicClientApplicationBuilder.Create(AppSettings.Settings.Auth.ClientId)
                    .WithIosKeychainSecurityGroup(AppSettings.Settings.IosKeychainSecurityGroups)
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
            AuthenticationService = new AuthenticationService(logger, AuthenticationClient, new SimpleOptions<AuthOptions>(auth));

            BraindumpRestClient = RestService.For<IBraindumpRestClient>(
                "https://nudelsieb.zoechbauer.dev/braindump/",
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
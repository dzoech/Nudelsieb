using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.Views;

namespace Nudelsieb.Mobile.Droid
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            App.UiParent = this;

            //FirebaseInstanceId.Instance.DeleteInstanceId();

            if (!IsPlayServiceAvailable())
            {
                throw new Exception("This device does not have Google Play Services and cannot receive push notifications.");
            }

            CreateNotificationChannel();
        }

        protected override void OnNewIntent(Intent intent)
        {
            Log.Debug(AppSettings.Settings.DebugTag, "OnNewIntent");

            if (intent.Extras != null)
            {
                var message = intent.GetStringExtra("message");
                (App.Current.MainPage as MainPage)?.AddMessage(message);
            }

            base.OnNewIntent(intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        private bool IsPlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Log.Debug(AppSettings.Settings.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    Log.Debug(AppSettings.Settings.DebugTag, "This device is not supported");
                }

                return false;
            }

            return true;
        }

        private void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo". There is no need to create a notification
            // channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = AppSettings.Settings.Notifications.NotificationChannelName;
                var channelDescription = "My channel description"; // string.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}

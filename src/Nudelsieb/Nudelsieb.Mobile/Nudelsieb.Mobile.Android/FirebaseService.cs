﻿using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Firebase.Messaging;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Views;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.Droid
{
    /// <summary>
    /// Documentation:
    /// https://firebase.google.com/docs/cloud-messaging/android/first-message?authuser=0#access_the_registration_token
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        private readonly IDeviceService deviceService;
        private readonly Random random = new Random();

        public FirebaseService()
        {
            deviceService = DependencyService.Resolve<IDeviceService>();
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            // NOTE: test messages sent via the Azure portal will be received here

            var dataNotification = new FcmDataNotification(message.Data);
            SendLocalNotification(dataNotification);

            // send the incoming message directly to the MainPage
            SendMessageToMainPage("Notification directly to MainPage");
        }

        /// <summary>
        /// There are two scenarios when onNewToken is called:
        /// 1) When a new token is generated on initial app startup
        /// 2) Whenever an existing token is changed
        /// Under #2, there are three scenarios when the existing token is changed:
        /// A) App is restored to a new device
        /// B) User uninstalls/reinstalls the app
        /// C) User clears app data
        /// </summary>
        public override void OnNewToken(string token)
        {
            SendRegistrationToServer(token);
        }

        private void SendLocalNotification(FcmDataNotification dataNotification)
        {
            // TODO do this in a platform specific service implementation

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", "TODO: Set via PutExtra('message')");
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            // prepend groups with a # sign
            var groups = string.Join(' ', dataNotification.Groups.Select(g => $"#{g}"));

            var notificationBuilder = new NotificationCompat.Builder(this, AppSettings.Settings.Notifications.NotificationChannelName)
                .SetContentTitle($"Reminder: {groups}")
                .SetContentText(dataNotification.NeuronInformation)
                .SetStyle(new NotificationCompat.BigTextStyle())
                //.SetContentInfo("ContentInfo")
                .SetSmallIcon(Resource.Drawable.icon)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .AddAction(new NotificationCompat.Action(0, "Snooze 1 hour", pendingIntent))
                .AddAction(new NotificationCompat.Action(0, "Snooze 1 day", pendingIntent))
                .AddAction(new NotificationCompat.Action(0, "Forget", pendingIntent))
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(AppSettings.Settings.Notifications.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(random.Next(), notificationBuilder.Build());
        }

        private void SendMessageToMainPage(string body)
        {
            (App.Current.MainPage as MainPage)?.AddMessage(body);
        }

        private void SendRegistrationToServer(string token)
        {
            try
            {
                // Just persist the token here for later use after the user has logged in.
                // Once the user has logged in, a device installation is created via nudelsieb's
                // Notifications endpoint.
                deviceService.SavePnsHandleAsync(token);
            }
            catch (Exception ex)
            {
                Log.Error(AppSettings.Settings.DebugTag, $"Error registering device for notifications: {ex.Message}");
            }
        }
    }
}

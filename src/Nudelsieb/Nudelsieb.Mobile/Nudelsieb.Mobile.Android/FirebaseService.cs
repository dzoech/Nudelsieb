﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Firebase.Messaging;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Views;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Forms;
using static Android.Provider.Settings;

namespace Nudelsieb.Mobile.Droid
{
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
        /// Only called once per installation.
        /// </summary>
        /// <param name="token"></param>
        public override void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired

            SendRegistrationToServer(token);
        }

        void SendLocalNotification(FcmDataNotification dataNotification)
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
        
        void SendMessageToMainPage(string body)
        {
            (App.Current.MainPage as MainPage)?.AddMessage(body);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                // just persist the token here for later use after the user has logged in
                deviceService.SavePnsHandle(token);
            }
            catch (Exception ex)
            {
                Log.Error(AppSettings.Settings.DebugTag, $"Error registering device: {ex.Message}");
            }
        }
    }
}
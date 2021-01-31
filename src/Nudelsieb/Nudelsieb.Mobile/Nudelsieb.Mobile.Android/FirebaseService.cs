using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Firebase.Messaging;
using Nudelsieb.Mobile.Views;
using WindowsAzure.Messaging;

namespace Nudelsieb.Mobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data.Values.First();
            }

            // convert the incoming message to a local notification
            SendLocalNotification(messageBody);

            // send the incoming message directly to the MainPage
            SendMessageToMainPage(messageBody);
        }

        public override void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired

            SendRegistrationToServer(token);
        }

        void SendLocalNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", body);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, AppSettings.Settings.Notifications.NotificationChannelName)
                .SetContentTitle("ContentTitle: Nudelsieb")
                .SetContentText("ContentText: " + body)
                //.SetContentInfo("ContentInfo")
                .SetSmallIcon(Resource.Drawable.xamarin_logo)
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
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        void SendMessageToMainPage(string body)
        {
            (App.Current.MainPage as MainPage)?.AddMessage(body);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(AppSettings.Settings.Notifications.NotificationHubName, AppSettings.Settings.Notifications.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, AppSettings.Settings.Notifications.SubscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", AppSettings.Settings.Notifications.FcmTemplateBody, AppSettings.Settings.Notifications.SubscriptionTags);
            }
            catch (Exception e)
            {
                Log.Error(AppSettings.Settings.DebugTag, $"Error registering device: {e.Message}");
            }
        }
    }
}
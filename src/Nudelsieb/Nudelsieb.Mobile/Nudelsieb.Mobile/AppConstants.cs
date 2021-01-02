﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Mobile
{
    public static class AppConstants
    {
        /// <summary>
        /// Notification channels are used on Android devices starting with "Oreo"
        /// </summary>
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";

        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        public static string NotificationHubName { get; set; } = "nudelsieb-notification-hub";

        /// <summary>
        /// This is the "DefaultListenSharedAccessSignature" connection string, which is
        /// found in your Azure Notification Hub portal under "Access Policies".
        /// 
        /// You should always use the ListenShared connection string. Do not use the
        /// FullShared connection string in a client application.
        /// </summary>
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://nudelsieb-notifications-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=yk25mflmcIHw7c5q+ayo+pq2GS6c+chC58mqxPKKT0k=";

        /// <summary>
        /// Tag used in log messages to easily filter the device log
        /// during development.
        /// </summary>
        public static string DebugTag { get; set; } = "XamarinNotify";

        /// <summary>
        /// The tags the device will subscribe to. These could be subjects like
        /// news, sports, and weather. Or they can be tags that identify a user
        /// across devices.
        /// </summary>
        public static string[] SubscriptionTags { get; set; } = { "default" };

        /// <summary>
        /// This is the template json that Android devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// </summary>
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";

        /// <summary>
        /// This is the template json that Apple devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// </summary>
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
    }
}

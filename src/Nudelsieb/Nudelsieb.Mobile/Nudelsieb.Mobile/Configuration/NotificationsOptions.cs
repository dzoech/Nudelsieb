namespace Nudelsieb.Mobile.Configuration
{
    public class NotificationsOptions
    {
        /// <summary>
        /// Notification channels are used on Android devices starting with "Oreo"
        /// </summary>
        public string NotificationChannelName { get; set; }

        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        public string NotificationHubName { get; set; }

        /// <summary>
        /// This is the "DefaultListenSharedAccessSignature" connection string, which is found in
        /// your Azure Notification Hub portal under "Access Policies". You should always use the
        /// ListenShared connection string. Do not use the FullShared connection string in a client
        /// application.
        /// </summary>
        public string ListenConnectionString { get; set; }

        /// <summary>
        /// The tags the device will subscribe to. These could be subjects like news, sports, and
        /// weather. Or they can be tags that identify a user across devices.
        /// </summary>
        public string[] SubscriptionTags { get; set; }

        /// <summary>
        /// This is the template json that Android devices will use. Templates are defined by the
        /// device and can include multiple parameters.
        /// </summary>
        public string FcmTemplateBody { get; set; }

        /// <summary>
        /// This is the template json that Apple devices will use. Templates are defined by the
        /// device and can include multiple parameters.
        /// </summary>
        public string ApnTemplateBody { get; set; }
    }
}

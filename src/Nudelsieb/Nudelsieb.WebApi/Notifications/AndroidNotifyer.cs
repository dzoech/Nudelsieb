using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nudelsieb.WebApi.Notifications
{
    public class AndroidNotifyer : IPushNotifyer
    {
        private readonly ILogger<AndroidNotifyer> logger;
        private readonly INotificationHubClient hub;
        private readonly IOptions<NotificationsOptions> hubOptions;


        public AndroidNotifyer(ILogger<AndroidNotifyer> logger, IOptions<NotificationsOptions> hubOptions)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // TODO: use DI
            this.hubOptions = hubOptions ?? throw new ArgumentNullException(nameof(hubOptions));
            this.hub = new NotificationHubClient(
                hubOptions.Value.AzureNotificationHub.ConnectionString,
                hubOptions.Value.AzureNotificationHub.HubName);

            this.hubOptions = hubOptions;
        }

        public async Task SubscribeAsync(DeviceInstallationDto installationRequest)
        {
            var userId = "ANY";

            var installation = new Installation
            {
                InstallationId = installationRequest.Id,
                PushChannel = installationRequest.PnsHandle,
                Tags = new[] { $"user:{userId}" },
                Platform = NotificationPlatform.Fcm,
            };

            // call notification hubs to create a new registration ID, and then return the ID back to the device.
            await hub.CreateOrUpdateInstallationAsync(installation);
        }

        public async Task UnsubscribeAsync(string id)
        {
            await hub.DeleteInstallationAsync(id);
        }

        public async Task<string> SendAsync(string message, string receiver)
        {
            var notification = new AndroidReminderBuilder()
                .WithNeuron(Guid.NewGuid(), message)
                .WithGroups("demo-group", "work", "project-nudelsieb")
                .Build();

            var x = await hub.GetAllRegistrationsAsync(10);

            var outcome = await hub.SendFcmNativeNotificationAsync(notification, tagExpression: $"user:{receiver}");
            logger.LogInformation($"Notified clients, tracking ID: {outcome.TrackingId}");

            return outcome.TrackingId;
        }
    }
}

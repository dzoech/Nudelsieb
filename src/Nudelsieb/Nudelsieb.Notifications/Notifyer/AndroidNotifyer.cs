using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Notifications;

namespace Nudelsieb.Notifications.Notifyer
{
    /// <summary>
    /// Sends push notifications to Android devices via the Azure Notifications Hub
    /// </summary>
    public class AndroidNotifyer : IPushNotifyer
    {
        private readonly ILogger<AndroidNotifyer> logger;
        private readonly INotificationHubClient hub;

        public AndroidNotifyer(ILogger<AndroidNotifyer> logger, INotificationHubClient hub)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.hub = hub;
        }

        public async Task SubscribeAsync(DeviceInstallationDto installationRequest, string user)
        {
            var userId = "ANY";

            var installation = new Installation
            {
                InstallationId = installationRequest.Id,
                UserId = user,
                PushChannel = installationRequest.PnsHandle,
                Tags = new[] { $"user:{userId}" },
                Platform = NotificationPlatform.Fcm,
            };

            // call notification hubs to create a new registration ID, and then return the ID back
            // to the device.
            await hub.CreateOrUpdateInstallationAsync(installation);
        }

        public async Task UnsubscribeAsync(string id, string user)
        {
            var installation = await hub.GetInstallationAsync(id);

            if (installation.UserId == user)
            {
                await hub.DeleteInstallationAsync(id);
            }
            else
            {
                throw new NotifyerException($"Installation id '{id}' does not belong user '{user}'");
            }
        }

        public async Task<string> SendAsync(string message, string receiver)
        {
            var deleteAllHandles = false;

            var regs = await hub.GetAllRegistrationsAsync(40);
            var regList = regs.ToList();

            if (deleteAllHandles)
            {
                foreach (var r in regList)
                {
                    await hub.DeleteRegistrationsByChannelAsync(r.PnsHandle);
                }
            }

            var notification = new AndroidReminderBuilder()
                .WithNeuron(Guid.NewGuid(), message)
                .WithGroups("demo-group", "work", "project-nudelsieb")
                .Build();

            var outcome = await hub.SendFcmNativeNotificationAsync(notification, tagExpression: $"user:{receiver}");
            logger.LogInformation($"Notified clients, tracking ID: {outcome.TrackingId}");

            return outcome.TrackingId;
        }
    }
}

﻿using System;
using System.Collections.Generic;
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

        public async Task<List<RegistrationDescription>> GetDebugInfo(bool deleteAllHandles = false)
        {
            var result = await hub.GetAllRegistrationsAsync(100);
            var registrations = result.ToList();
            logger.LogInformation($"Number of registered devices: {registrations.Count}");

            if (deleteAllHandles)
            {
                foreach (var r in registrations)
                {
                    logger.LogWarning("Deleting registration for handle '{pnsHandle}'", r.PnsHandle);
                    await hub.DeleteRegistrationsByChannelAsync(r.PnsHandle);
                }
            }

            return registrations;
        }

        public async Task SubscribeAsync(DeviceInstallationDto installationRequest, Guid userId)
        {
            var installation = new Installation
            {
                InstallationId = installationRequest.Id,
                UserId = userId.ToString(),
                PushChannel = installationRequest.PnsHandle,
                Platform = NotificationPlatform.Fcm,
            };

            // call notification hubs to create a new registration ID, and then return the ID back
            // to the device.
            await hub.CreateOrUpdateInstallationAsync(installation);
        }

        public async Task UnsubscribeAsync(string installationId, Guid userId)
        {
            var installation = await hub.GetInstallationAsync(installationId);

            if (installation.UserId == userId.ToString())
            {
                await hub.DeleteInstallationAsync(installationId);
                logger.LogInformation("Deleted installation {installationId} for user {user}", installationId, userId);
            }
            else
            {
                throw new NotifyerException($"Installation id '{installationId}' does not belong user '{userId}'");
            }
        }

        public async Task<string> SendAsync(string message, Guid receiverId)
        {
            var notification = new AndroidReminderBuilder()
                .WithNeuron(Guid.NewGuid(), message)
                .WithGroups("demo-group", "work", "project-nudelsieb")
                .Build();

            logger.LogInformation("Notification to be sent: '{notification}'", notification);

            string receiverTag = GetTagForUserId(receiverId);
            var outcome = await hub.SendFcmNativeNotificationAsync(notification, receiverTag);

            logger.LogInformation(
                "Notified clients by tag expression '{tagExpression}', tracking id: '{trackingId}'",
                receiverTag,
                outcome.TrackingId);

            return outcome.TrackingId;
        }

        /// <summary>
        /// An installation for the Notifications Hub automatically creates a tag for the defined user id.
        /// Example tag: $UserId:{93fab134-e9ed-4a73-a106-ba6fa15f7f20}
        /// </summary>
        private string GetTagForUserId(Guid userId) => "$UserId:{" + userId + "}";
    }
}

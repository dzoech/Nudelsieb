using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nudelsieb.WebApi.Notifications
{
    [Area("notifications")]
    [Route("[area]/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        private readonly IOptions<AzureNotificationHubOptions> hubOptions;
        private readonly ILogger<RegistrationController> logger;
        private readonly NotificationHubClient hub;

        public RegistrationController(
            IOptions<AzureNotificationHubOptions> hubOptions, 
            ILogger<RegistrationController> logger)
        {
            this.hubOptions = hubOptions ?? throw new ArgumentNullException(nameof(hubOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // TODO: use DI
            hub = new NotificationHubClient(hubOptions.Value.ConnectionString, hubOptions.Value.HubName);
        }

        /// <summary>
        /// Creates or updates an installation for the authorized user. The consumer
        /// is responsible for storing the installation id.
        /// </summary>
        [HttpPut]
        public async Task UpdateInstallation(DeviceInstallationDto installationRequest)
        {
            var userId = "ANY";

            var installation = new Installation
            {
                InstallationId = installationRequest.Id,
                PushChannel = installationRequest.PnsHandle,
                Tags = { $"user:{userId}" },
                Platform = NotificationPlatform.Fcm,
            };

            // call notification hubs to create a new registration ID, and then return the ID back to the device.
            await hub.CreateOrUpdateInstallationAsync(installation);
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstallation(string id)
        {
            await hub.DeleteInstallationAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Notify([FromQuery] string receiver, [FromQuery] string message)
        {

            var payload = "{\"data\": {\"action\": \"Movement\"},\"notification\": {\"title\": \"This is a title\",\"body\": \"test message\"}}";

            var outcome = await hub.SendFcmNativeNotificationAsync(payload, tagExpression: "user:ANY");
            logger.LogInformation($"Notified {outcome.Success} clients");
        }
    }
}

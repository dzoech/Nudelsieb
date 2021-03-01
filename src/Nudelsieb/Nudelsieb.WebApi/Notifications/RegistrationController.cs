using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.WebApi.Notifications.Notifyer;

namespace Nudelsieb.WebApi.Notifications
{
    [Area("notifications")]
    [Route("[area]/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> logger;
        private readonly IPushNotifyer pushNotifyer;

        public RegistrationController(
            ILogger<RegistrationController> logger, 
            IPushNotifyer notificationSender)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.pushNotifyer = notificationSender ?? throw new ArgumentNullException(nameof(notificationSender));
        }

        /// <summary>
        /// Creates or updates an installation for the authorized user. The consumer
        /// is responsible for storing the installation id.
        /// </summary>
        [HttpPut]
        public async Task UpdateInstallationAsync(DeviceInstallationDto installationRequest)
        {
            await pushNotifyer.SubscribeAsync(installationRequest, "dominik");
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstallationAsync(string id)
        {
            await pushNotifyer.UnsubscribeAsync(id, "dominik");
        }

        /// <summary>
        /// Sends test notifications to registered receivers.
        /// </summary>
        /// <param name="receiver" example="ANY"></param>
        /// <param name="message" example="This is an example notification sent via the REST API"></param>
        /// <returns></returns>
        [HttpPost("~/[area]")]
        [AllowAnonymous]
        public async Task<string> Notify([FromQuery] string receiver, [FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(receiver))
            {
                receiver = "ANY";
            }

            var trackingId = await pushNotifyer.SendAsync(message, receiver);

            return $"Tracking ID: {trackingId}";
        }
    }
}

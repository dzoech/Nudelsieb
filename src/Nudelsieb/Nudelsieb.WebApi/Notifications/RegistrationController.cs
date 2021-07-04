using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Notifications;

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
        private readonly INotificationScheduler notificationScheduler;

        public RegistrationController(
            ILogger<RegistrationController> logger,
            IPushNotifyer notificationSender,
            INotificationScheduler notificationScheduler)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.pushNotifyer = notificationSender ?? throw new ArgumentNullException(nameof(notificationSender));
            this.notificationScheduler = notificationScheduler ?? throw new ArgumentNullException(nameof(notificationScheduler));
        }

        /// <summary>
        /// Creates or updates an installation for the authorized user. The consumer is responsible
        /// for storing the installation id.
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
        /// Sends test notifications to registered receivers. This is used for developing.
        /// </summary>
        /// <param name="receiver" example="ANY"></param>
        /// <param name="message" example="This is an example notification sent via the REST API"></param>
        /// <param name="delayInSeconds">
        /// Specifies how many seconds to wait before pushing the notification to the receiving
        /// devices.
        /// </param>
        /// <param name="delayInMinutes">
        /// Specifies additional minutes to wait before pushing the notification to the receiving
        /// devices.
        /// </param>
        [HttpPost("~/[area]")]
        [AllowAnonymous]
        public async Task Notify([FromQuery] string receiver, [FromQuery] string message, [FromQuery] int delayInSeconds, [FromQuery] int delayInMinutes)
        {
            if (string.IsNullOrWhiteSpace(receiver))
            {
                receiver = "ANY";
            }

            var scheduleAt = DateTimeOffset.Now
                .AddSeconds(delayInSeconds)
                .AddMinutes(delayInMinutes);

            await notificationScheduler.ScheduleAsync(message, scheduleAt);
        }
    }
}

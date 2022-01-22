using System;
using System.Security.Claims;
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
        [AllowAnonymous]
        public async Task UpdateInstallationAsync(DeviceInstallationDto installationRequest)
        {
            var userId = HttpContext.User.FindFirstValue("sub"); // is always null -> #todo fix auth pipeline
            if (userId is null)
            {
                logger.LogWarning("No user id (sub claim) has been provided with the installation request");
                userId = "not-set";
            }

            await pushNotifyer.SubscribeAsync(installationRequest, userId);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task DeleteInstallationAsync(string id)
        {
            var userId = HttpContext.User.FindFirstValue("sub");
            if (userId is null)
            {
                logger.LogWarning("No user id (sub claim) has been provided with the installation request");
                userId = "not-set"; // this will break the unsubscribing
            }

            await pushNotifyer.UnsubscribeAsync(id, userId);
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
        [HttpPost("~/debug/[area]")]
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

        [HttpGet("~/debug/registrations")]
        //#if !DEBUG
        [Authorize]
        //#endif
        public async Task<ActionResult> GetDebugInfo()
        {
            var res = await ((Nudelsieb.Notifications.Notifyer.AndroidNotifyer)pushNotifyer).GetDebugInfo();
            return Ok(res);
        }
    }
}

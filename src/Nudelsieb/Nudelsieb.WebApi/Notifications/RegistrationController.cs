using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Notifications;

namespace Nudelsieb.WebApi.Notifications
{
    [Authorize]
    [Area("notifications")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> logger;
        private readonly UserService userService;
        private readonly IPushNotifyer pushNotifyer;
        private readonly INotificationScheduler notificationScheduler;

        public RegistrationController(
            ILogger<RegistrationController> logger,
            UserService userService,
            IPushNotifyer notificationSender,
            INotificationScheduler notificationScheduler)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
            var userId = userService.GetActiveUserId();
            await pushNotifyer.SubscribeAsync(installationRequest, userId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstallationAsync(string id)
        {
            var userId = userService.GetActiveUserId();
            await pushNotifyer.UnsubscribeAsync(id, userId);
        }

        /// <summary>
        /// Sends test notifications to registered receivers. This is used for developing.
        /// </summary>
        /// <param name="receiver" example="ANY">The id of the receiving user.</param>
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
#if DEBUG
        [AllowAnonymous]
#else
        [Authorize]
#endif
        public async Task Notify([FromQuery] Guid receiver, [FromQuery] string message, [FromQuery] int delayInSeconds, [FromQuery] int delayInMinutes)
        {
            var scheduleAt = DateTimeOffset.Now
                .AddSeconds(delayInSeconds)
                .AddMinutes(delayInMinutes);

            await notificationScheduler.ScheduleAsync(message, receiver, scheduleAt);
        }

        [HttpGet("~/debug/registrations")]
#if DEBUG
        [AllowAnonymous]
#else
        [Authorize]
#endif
        public async Task<ActionResult> GetDebugInfo()
        {
            var res = await ((Nudelsieb.Notifications.Notifyer.AndroidNotifyer)pushNotifyer).GetDebugInfo();
            return Ok(res);
        }
    }
}

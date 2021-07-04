namespace Nudelsieb.Application.Notifications
{
    public class DeviceInstallationDto
    {
        /// <summary>
        /// The id of the installation.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The platform to which this installation belongs to.
        /// </summary>
        /// <example>FCM</example>
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// The platform specific handle to which notifications are sent to. A handle is also called
        /// token.
        /// </summary>
        public string PnsHandle { get; set; } = string.Empty;
    }
}

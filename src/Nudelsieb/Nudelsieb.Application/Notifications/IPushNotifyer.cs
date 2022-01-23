using System;
using System.Threading.Tasks;

namespace Nudelsieb.Application.Notifications
{
    public interface IPushNotifyer
    {
        Task SubscribeAsync(DeviceInstallationDto installationRequest, Guid userId);

        Task UnsubscribeAsync(string id, Guid userId);

        Task<string> SendAsync(string message, Guid receiverId);
    }
}

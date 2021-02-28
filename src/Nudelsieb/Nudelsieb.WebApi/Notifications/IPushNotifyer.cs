using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Notifications
{
    public interface IPushNotifyer
    {
        Task SubscribeAsync(DeviceInstallationDto installationRequest);
        Task UnsubscribeAsync(string id);
        Task<string> SendAsync(string message, string receiver);
    }
}
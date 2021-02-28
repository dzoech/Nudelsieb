using System.Threading.Tasks;
using Refit;

namespace Nudelsieb.Shared.Clients.Notifications
{
    [Headers("User-Agent: Nudelsieb.Shared.Clients", "Authorization: Bearer")]
    public interface INotificationsRestClient
    {
        [Put("/Registration")]
        public Task RegisterDeviceAsync(DeviceInstallation installation);
    }
}

using System.Threading.Tasks;
using Nudelsieb.Shared.Clients.Notifications;

namespace Nudelsieb.Mobile.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// Returns the locally stored pns handle.
        /// </summary>
        Task<string> GetPnsHandleAsync();

        /// <summary>
        /// Updates the stored token for the PNS handle to match the current token assigned by the
        /// platfrom.
        /// </summary>
        /// <returns>Whether the locally stored token has been updated.</returns>
        Task<bool> RefreshPnsHandleAsync();

        Task SavePnsHandleAsync(string handle);

        void ClearPnsHandle();

        string GetDeviceId();

        Task<DeviceInstallation> GetDeviceInstallation();
    }
}

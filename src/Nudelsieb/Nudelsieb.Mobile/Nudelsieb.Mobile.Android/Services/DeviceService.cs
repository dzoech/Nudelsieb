using System.Threading.Tasks;
using Android.App;
using Android.Gms.Extensions;
using Firebase.Installations;
using Firebase.Messaging;
using Nudelsieb.Mobile.Droid.Services;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Essentials;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceService))]

namespace Nudelsieb.Mobile.Droid.Services
{
    public class DeviceService : IDeviceService
    {
        public const string PnsHandleKey = "nds.device.pnshandle";
        public const string FirebaseInstallationIdKey = "nds.device.pnshandle.installation.id";
        private const string NotificationPlatform = "fcm";

        public async Task<string> GetPnsHandleAsync()
        {
            return await SecureStorage.GetAsync(PnsHandleKey);
        }

        public async Task<bool> RefreshPnsHandleAsync()
        {
            var javaFcmToken = await FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
            string fcmToken = javaFcmToken.ToString();
            string storedToken = await SecureStorage.GetAsync(PnsHandleKey);
            if (storedToken != fcmToken)
            {
                await SavePnsHandleAsync(fcmToken);
                return true;
            }

            return false;
        }

        public async Task SavePnsHandleAsync(string handle)
        {
            await SecureStorage.SetAsync(PnsHandleKey, handle);

            var installationId = await FirebaseInstallations.Instance.GetId().AsAsync<Java.Lang.String>();
            await SecureStorage.SetAsync(FirebaseInstallationIdKey, installationId.ToString());
        }

        public void ClearPnsHandle()
        {
            SecureStorage.Remove(PnsHandleKey);
        }

        public async Task<DeviceInstallation> GetDeviceInstallation()
        {
            var thisDevice = new DeviceInstallation
            {
                Id = GetDeviceId(),
                Platfrom = NotificationPlatform,
                PnsHandle = await GetPnsHandleAsync(),
            };

            return thisDevice;
        }

        public string GetDeviceId()
        {
            return Secure.GetString(Application.Context.ContentResolver, Secure.AndroidId);
        }
    }
}

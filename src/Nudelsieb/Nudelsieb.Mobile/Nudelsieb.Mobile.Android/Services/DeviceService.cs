using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Nudelsieb.Mobile.Droid.Services;
using Nudelsieb.Mobile.Services;
using Xamarin.Essentials;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceService))]
namespace Nudelsieb.Mobile.Droid.Services
{
    public class DeviceService : IDeviceService
    {
        public const string PnsHandleKey = "nds.device.pnshandle";

        public async Task<string> GetPnsHandleAsync()
        {
            return await SecureStorage.GetAsync(PnsHandleKey);
        }

        public async Task SavePnsHandleAsync(string handle)
        {
            await SecureStorage.SetAsync(PnsHandleKey, handle);
        }

        public void ClearPnsHandle()
        {
            SecureStorage.Remove(PnsHandleKey);
        }

        public string GetDeviceId()
        {
            return Secure.GetString(Application.Context.ContentResolver, Secure.AndroidId);
        }
    }
}

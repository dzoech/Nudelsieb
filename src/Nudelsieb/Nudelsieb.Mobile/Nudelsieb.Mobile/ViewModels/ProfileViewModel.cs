using System;
using System.Linq;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Services;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        private string _name;
        private string _email;
        private string _pnsHandle;
        private string _deviceId;
        private string _registrationId;
        private string _installationId;
        private string _subClaim;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string SubClaim
        {
            get => _subClaim;
            set => SetProperty(ref _subClaim, value);
        }

        public string DeviceId
        {
            get => _deviceId;
            set => SetProperty(ref _deviceId, value);
        }

        public string PnsHandle
        {
            get => _pnsHandle;
            set => SetProperty(ref _pnsHandle, value);
        }

        public string RegistrationId
        {
            get => _registrationId;
            set => SetProperty(ref _registrationId, value);
        }

        public string InstallationId
        {
            get => _installationId;
            set => SetProperty(ref _installationId, value);
        }

        public async Task<bool> Refresh()
        {
            try
            {
                (var success, var token) = await App.AuthenticationService.GetCachedAccessTokenAsync();

                if (success)
                {
                    IsLoggedIn = true;
                    Name = token.Claims.SingleOrDefault(c => c.Type == "given_name")?.Value;
                    Email = token.Claims.SingleOrDefault(c => c.Type == "email")?.Value;
                    SubClaim = token.Claims.SingleOrDefault(c => c.Type == "sub")?.Value;

                    var deviceService = DependencyService.Resolve<IDeviceService>();
                    var hasPnsHandleChanged = await deviceService.RefreshPnsHandleAsync();

                    if (hasPnsHandleChanged)
                    {
                        Alerter.Alert("Updated locally stored PNS handle.");
                    }

                    var deviceInstallation = await deviceService.GetDeviceInstallation();

                    await App.NotificationsRestClient.RegisterDeviceAsync(deviceInstallation);

                    DeviceId = deviceInstallation.Id;
                    PnsHandle = deviceInstallation.PnsHandle;
                }
                else
                {
                    await Shell.Current.GoToAsync(@"//login");
                }
            }
            catch (Exception ex)
            {
                Alerter.Alert(ex.GetType().Name + ": " + ex.Message);
            }

            return IsLoggedIn;
        }
    }
}

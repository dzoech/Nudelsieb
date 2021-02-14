using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Shared.Clients.Authentication;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class LoginViewModel
    {
        private IAuthenticationService _authenticationService;

        public Command LoginCommand { get; set; }

        public LoginViewModel()
            : this(App.AuthenticationService)
        {
        }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            LoginCommand = new Command(async () => await LoginAsync());
            _authenticationService = authenticationService;
        }

        private async Task LoginAsync()
        {
            try
            {
                await _authenticationService.LoginAsync();

                var deviceService = DependencyService.Resolve<IDeviceService>();
                var deviceId = deviceService.GetDeviceId();
                var handle = await deviceService.GetHandleAsync();

                var thisDevice = new DeviceInstallation
                {
                    Id = deviceId,
                    Platfrom = "fcm",
                    PnsHandle = handle,
                };

                await App.NotificationsRestClient.RegisterDeviceAsync(thisDevice);

                await Shell.Current.GoToAsync(@"//main");
            }
            catch (Exception ex)
            {
                var alerter = DependencyService.Resolve<IAlerter>();
                alerter.Alert($"Login failed. {ex.Message}");
            }
        }
    }
}

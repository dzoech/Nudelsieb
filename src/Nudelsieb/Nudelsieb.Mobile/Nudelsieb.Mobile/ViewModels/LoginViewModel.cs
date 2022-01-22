using System;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Shared.Clients.Authentication;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class LoginViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel()
            : this(App.AuthenticationService)
        {
        }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            LoginCommand = new Command(async () => await LoginAsync());
            _authenticationService = authenticationService;
        }

        public Command LoginCommand { get; set; }
        private async Task LoginAsync()
        {
            try
            {
                await _authenticationService.LoginAsync();

                var deviceService = DependencyService.Resolve<IDeviceService>();
                var deviceId = deviceService.GetDeviceId();
                var handle = await deviceService.GetPnsHandleAsync();

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

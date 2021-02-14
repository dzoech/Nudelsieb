using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Shared.Clients.Authentication;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    class LoadingViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public LoadingViewModel() : this(null)
        {
        }

        public LoadingViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? App.AuthenticationService;
        }

        public async Task Init()
        {
            (var isAuthenticated, _) = await _authenticationService.GetCachedAccessTokenAsync();
            if (isAuthenticated)
            {
                await Shell.Current.GoToAsync(@"//main");
            }
            else
            {
                await Shell.Current.GoToAsync(@"//login");
            }
        }
    }
}

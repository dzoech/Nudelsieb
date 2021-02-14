using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Views;
using Nudelsieb.Shared.Clients.Notifications;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        private string _name;
        private string _email;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public string Name
        {
            get => _name;
            set { SetProperty(ref _name, value); }

        }

        public string Email
        {
            get => _email;
            set { SetProperty(ref _email, value); }

        }

        public async Task<bool> Refresh()
        {
            (var success, var token) = await App.AuthenticationService.GetCachedAccessTokenAsync();

            if (success)
            {
                IsLoggedIn = true;
                Name = token.Claims.SingleOrDefault(c => c.Type == "given_name")?.Value;
                Email = token.Claims.SingleOrDefault(c => c.Type == "email")?.Value;
            }
            else
            {
                await Shell.Current.GoToAsync(@"//login");
            }

            return IsLoggedIn;
        }
    }
}

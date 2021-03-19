using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Nudelsieb.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new ProfileViewModel();
        }

        protected override async void OnAppearing()
        {
            await _viewModel.Refresh();
            base.OnAppearing();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            AuthenticationResult result;
            try
            {
                //result = await App.AuthenticationClient
                //    .AcquireTokenInteractive(AppSettings.Settings.Auth.RequiredScopes)
                //    .WithPrompt(Prompt.SelectAccount)
                //    .WithParentActivityOrWindow(App.UiParent)
                //    .ExecuteAsync();

                //await Navigation.PushAsync(new LogoutPage(result));
            }
            catch (MsalException ex)
            {
                if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                {
                    await Navigation.PushAsync(new ProfilePage());
                }
                else if (ex.ErrorCode != "authentication_canceled")
                {
                    await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }
    }
}
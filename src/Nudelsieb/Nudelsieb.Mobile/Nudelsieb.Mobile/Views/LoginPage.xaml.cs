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
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            await viewModel.Refresh();
            //try
            //{
            //    // Look for existing account
            //    IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();

            //    if (accounts.Count() == 0)
            //        return;

            //    AuthenticationResult result = await App.AuthenticationClient
            //        .AcquireTokenSilent(AppSettings.Settings.Auth.RequiredScopes, accounts.FirstOrDefault())
            //        .ExecuteAsync();

            //    await Navigation.PushAsync(new MainPage());
            //}
            //catch
            //{
            //    // Do nothing - the user isn't logged in
            //}

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
                    await Navigation.PushAsync(new LogoutPage());
                }
                else if (ex.ErrorCode != "authentication_canceled")
                {
                    await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }


    }
}
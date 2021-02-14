using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new LoginViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            // block leaving this dialog without signing in
            return true;
        }
    }
}
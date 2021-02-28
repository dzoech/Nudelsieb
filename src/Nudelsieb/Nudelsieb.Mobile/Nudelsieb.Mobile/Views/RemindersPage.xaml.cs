using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Models;
using Nudelsieb.Mobile.ViewModels;
using Nudelsieb.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views
{
    public partial class RemindersPage : ContentPage
    {
        RemindersViewModel _viewModel;

        public RemindersPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RemindersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
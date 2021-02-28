using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nudelsieb.Mobile.Models;
using Nudelsieb.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
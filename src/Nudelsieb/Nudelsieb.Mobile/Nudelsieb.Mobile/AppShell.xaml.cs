using System;
using System.Collections.Generic;
using Nudelsieb.Mobile.ViewModels;
using Nudelsieb.Mobile.Views;
using Xamarin.Forms;

namespace Nudelsieb.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

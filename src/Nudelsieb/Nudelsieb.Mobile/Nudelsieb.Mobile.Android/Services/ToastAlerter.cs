using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Nudelsieb.Mobile.Droid.Services;
using Nudelsieb.Mobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAlerter))]
namespace Nudelsieb.Mobile.Droid.Services
{
    public class ToastAlerter : IAlerter
    {
        public void Alert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}
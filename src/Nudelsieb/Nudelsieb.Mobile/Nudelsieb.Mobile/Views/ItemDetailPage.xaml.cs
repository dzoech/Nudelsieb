using System.ComponentModel;
using Nudelsieb.Mobile.ViewModels;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
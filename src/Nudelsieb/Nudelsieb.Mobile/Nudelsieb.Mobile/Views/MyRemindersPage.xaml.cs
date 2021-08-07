using Nudelsieb.Mobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views
{
    /// <summary>
    /// Page to show recent chat list.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRemindersPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyRemindersPage" /> class.
        /// </summary>
        public MyRemindersPage()
        {
            this.InitializeComponent();
            this.BindingContext = MyRemindersViewModel.BindingContext;
        }
    }
}

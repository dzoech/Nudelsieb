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
        private readonly MyRemindersViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyRemindersPage" /> class.
        /// </summary>
        public MyRemindersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MyRemindersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}

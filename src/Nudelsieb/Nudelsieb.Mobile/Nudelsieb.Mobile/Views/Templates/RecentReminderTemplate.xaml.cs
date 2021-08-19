using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Nudelsieb.Mobile.Views.Templates
{
    /// <summary>
    /// Recent reminder page item's template.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentReminderTemplate : Grid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecentReminderTemplate"/> class.
        /// </summary>
        public RecentReminderTemplate()
        {
            this.InitializeComponent();
        }
    }
}

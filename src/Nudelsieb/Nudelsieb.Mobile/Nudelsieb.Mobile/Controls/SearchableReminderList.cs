using System;
using System.Linq;
using System.Threading.Tasks;
using Nudelsieb.Mobile.RestClients.Models;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Nudelsieb.Mobile.Controls
{
    /// <summary>
    /// This class extends the behavior of the SfListView control to filter the ListViewItem based
    /// on text.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SearchableReminderList : SearchableListView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchableReminderList"/> class.
        /// </summary>
        public SearchableReminderList()
        {
            this.SelectionChanged -= this.CustomListView_SelectionChanged;
            this.SelectionChanged += this.CustomListView_SelectionChanged;
        }

        /// <summary>
        /// Filtering the list view items based on the search text.
        /// </summary>
        /// <param name="obj">The list view item</param>
        /// <returns>Returns the filtered item</returns>
        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                if (!(obj is Reminder reminder))
                {
                    return false;
                }

                bool groupsContainSearchText = reminder.NeuronGroups
                    .Any(g => g.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));

                return
                    reminder.NeuronInformation.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    groupsContainSearchText;
            }

            return false;
        }

        /// <summary>
        /// Invoked when list view selection items are changed.
        /// </summary>
        /// <param name="sender">The ListView</param>
        /// <param name="e">Item selection changed event args</param>
        private async void CustomListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            Application.Current.Resources.TryGetValue("Gray-100", out var retVal);
            this.SelectionBackgroundColor = (Color)retVal;
            await Task.Delay(100).ConfigureAwait(true);
            this.SelectionBackgroundColor = Color.Transparent;
            this.SelectedItems.Clear();
        }
    }
}

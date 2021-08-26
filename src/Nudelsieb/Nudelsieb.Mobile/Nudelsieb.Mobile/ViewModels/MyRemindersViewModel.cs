using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Models;
using Nudelsieb.Mobile.RestClients.Models;
using Nudelsieb.Mobile.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Nudelsieb.Mobile.ViewModels
{
    /// <summary>
    /// View model for recent chat page
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class MyRemindersViewModel : BaseViewModel
    {
        private Command itemSelectedCommand;
        private Command makeVoiceCallCommand;
        private Command makeVideoCallCommand;
        private Command showSettingsCommand;
        private Command menuCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyRemindersViewModel"/> class.
        /// </summary>
        public MyRemindersViewModel()
        {
            ReminderItems = new ObservableCollection<Reminder>();
            LoadRemindersCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
        }

        public ObservableCollection<Reminder> ReminderItems { get; }
        public Command LoadRemindersCommand { get; }

        /// <summary>
        /// Gets the command that is executed when the voice call button is clicked.
        /// </summary>
        public Command MakeVoiceCallCommand => makeVoiceCallCommand ??= new Command(VoiceCallClicked);

        /// <summary>
        /// Gets the command that is executed when the video call button is clicked.
        /// </summary>
        public Command MakeVideoCallCommand => makeVideoCallCommand ??= new Command(VideoCallClicked);

        /// <summary>
        /// Gets the command that is executed when the settings button is clicked.
        /// </summary>
        public Command ShowSettingsCommand => showSettingsCommand ??= new Command(SettingsClicked);

        /// <summary>
        /// Gets the command that is executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand => menuCommand ??= new Command(MenuClicked);

        /// <summary>
        /// Gets the command that is executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand => itemSelectedCommand ??= new Command(ItemSelected);

        private async Task ExecuteLoadItemsCommandAsync()
        {
            IsBusy = true;

            try
            {
                var reminders = await App.BraindumpRestClient
                    .GetRemindersAsync(DateTimeOffset.Now + TimeSpan.FromDays(14));

                ReminderItems.ReplaceWith(reminders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Alerter.Alert(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the voice call button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void VoiceCallClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the video call button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void VideoCallClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the settings button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SettingsClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the menu button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void MenuClicked(object obj)
        {
            // Do something
        }
    }
}

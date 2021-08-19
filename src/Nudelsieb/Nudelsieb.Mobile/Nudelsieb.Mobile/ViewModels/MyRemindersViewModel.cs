using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using Nudelsieb.Mobile.Models;
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
        private static MyRemindersViewModel recentChatViewModel;

        private ObservableCollection<ReminderDetail> chatItems;

        private string profileImage;

        private Command itemSelectedCommand;

        private Command makeVoiceCallCommand;

        private Command makeVideoCallCommand;

        private Command showSettingsCommand;

        private Command menuCommand;

        private Command profileImageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyRemindersViewModel"/> class.
        /// </summary>
        public MyRemindersViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the value of recent chat page view model.
        /// </summary>
        public static MyRemindersViewModel BindingContext =>
            recentChatViewModel = PopulateData<MyRemindersViewModel>("chat.json");

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        [DataMember(Name = "profileImage")]
        public string ProfileImage
        {
            get
            {
                return App.ImageServerPath + this.profileImage;
            }

            set
            {
                this.SetProperty(ref this.profileImage, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the
        /// profile items.
        /// </summary>
        [DataMember(Name = "chatItems")]
        public ObservableCollection<ReminderDetail> ChatItems
        {
            get
            {
                return this.chatItems;
            }

            set
            {
                if (this.chatItems == value)
                {
                    return;
                }

                this.SetProperty(ref this.chatItems, value);
            }
        }

        /// <summary>
        /// Gets the command that is executed when the voice call button is clicked.
        /// </summary>
        public Command MakeVoiceCallCommand
        {
            get { return this.makeVoiceCallCommand ?? (this.makeVoiceCallCommand = new Command(this.VoiceCallClicked)); }
        }

        /// <summary>
        /// Gets the command that is executed when the video call button is clicked.
        /// </summary>
        public Command MakeVideoCallCommand
        {
            get { return this.makeVideoCallCommand ?? (this.makeVideoCallCommand = new Command(this.VideoCallClicked)); }
        }

        /// <summary>
        /// Gets the command that is executed when the settings button is clicked.
        /// </summary>
        public Command ShowSettingsCommand
        {
            get { return this.showSettingsCommand ?? (this.showSettingsCommand = new Command(this.SettingsClicked)); }
        }

        /// <summary>
        /// Gets the command that is executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand
        {
            get { return this.menuCommand ?? (this.menuCommand = new Command(this.MenuClicked)); }
        }

        /// <summary>
        /// Gets the command that is executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        }

        /// <summary>
        /// Gets the command that is executed when the profile image is clicked.
        /// </summary>
        public Command ProfileImageCommand
        {
            get { return this.profileImageCommand ?? (this.profileImageCommand = new Command(this.ProfileImageClicked)); }
        }

        /// <summary>
        /// Populates the data for view model from json file.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="fileName">Json file to fetch data.</param>
        /// <returns>Returns the view model object.</returns>
        private static T PopulateData<T>(string fileName)
        {
            var file = "Nudelsieb.Mobile.Data." + fileName;
            var assembly = typeof(App).GetTypeInfo().Assembly;
            T data;

            try
            {
                using (var stream = assembly.GetManifestResourceStream(file))
                {
                    if (stream is null)
                    {
                        throw new System.Exception(
                            $"Could not find manifest resource");
                    }

                    using var reader = new System.IO.StreamReader(stream);
                    var dataString = reader.ReadToEnd();

                    var options = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true,
                        PropertyNameCaseInsensitive = true
                    };

                    data = JsonSerializer.Deserialize<T>(dataString, options);
                    return data;
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Profile image is clicked.
        /// </summary>
        private void ProfileImageClicked(object obj)
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

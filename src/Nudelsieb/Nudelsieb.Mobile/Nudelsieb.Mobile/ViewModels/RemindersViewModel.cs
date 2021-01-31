using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Nudelsieb.Mobile.Models;
using Nudelsieb.Mobile.RestClients.Models;
using Nudelsieb.Mobile.Services;
using Nudelsieb.Mobile.Utils;
using Nudelsieb.Mobile.Views;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class RemindersViewModel : BaseViewModel
    {
        private Reminder _selectedItem;

        public ObservableCollection<Reminder> Reminders { get; }
        public Command LoadRemindersCommand { get; }
        public Command AddReminderCommand { get; }
        public Command<Reminder> ItemTapped { get; }

        public RemindersViewModel()
        {
            Title = "Browse";
            Reminders = new ObservableCollection<Reminder>();
            LoadRemindersCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Reminder>(OnItemSelected);

            AddReminderCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var reminders = await App.BraindumpRestClient.GetRemindersAsync(DateTimeOffset.Now + TimeSpan.FromDays(14));
                Reminders.ReplaceWith(reminders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                DependencyService.Get<IAlerter>().Alert(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Reminder SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Reminder item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Nudelsieb.Mobile.Models;
using Nudelsieb.Mobile.Services;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy = false;
        private string title = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IAlerter Alerter => DependencyService.Get<IAlerter>();
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

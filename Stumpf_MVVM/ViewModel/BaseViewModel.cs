using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Stumpf_MVVM.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected T SetProperty<T>(T backingStore, T value, Command onChanged = null, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return backingStore;
            onChanged?.ChangeCanExecute();
            OnPropertyChanged(propertyName);
            return value;
        }
    }

    public static class ObservableExtensionMethods
    {
        public static void SincronizarColecoes<T>(this ObservableCollection<T> destino, List<T> origem)
        {
            for (int i = 0; i < origem.Count; i++)
            {
                if (destino.Count <= i)
                    destino.Add(origem[i]);
                else if (!destino[i].Equals(origem[i]))
                    destino[i] = origem[i];
                else
                {
                    var notificarListView = destino[i].GetType().GetProperty("NotificarListView").GetValue(destino[i], null);
                    if (notificarListView != null && (bool)notificarListView)
                        destino[i] = origem[i];
                }
            }

            for (int i = origem.Count; i < destino.Count; i++)
            {
                destino.RemoveAt(i);
            }
        }
    }
}

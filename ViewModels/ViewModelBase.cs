using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkTime.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            // Empty method to handle unsubscribe events in ViewModels
        }
    }
}

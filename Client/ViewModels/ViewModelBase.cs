using System.ComponentModel;
using Client.Models;

namespace Client.ViewModels
{
    // public class ViewModelBase : ObservableObject
    // {
    // }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

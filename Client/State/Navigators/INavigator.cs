using System.Windows.Input;
using Client.ViewModels;

namespace Client.State.Navigators
{
    public enum ViewType
    {
        Home,
        Teams,
        Join,
        Calendar
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }

        ViewModelBase GetModelFromViewType(ViewType type);
    }
}

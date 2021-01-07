using System.Windows.Input;
using Client.ViewModels;

namespace Client.State.Navigators
{
    public enum ViewType
    {
        Home,
        Teams,
        Create,
        Calendar,
        Login,
        Register
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}

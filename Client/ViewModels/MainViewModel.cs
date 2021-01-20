using Client.State.Navigators;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; } = new Navigator();

        public MainViewModel ()
        {
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }
    }
}

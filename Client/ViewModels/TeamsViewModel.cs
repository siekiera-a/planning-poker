using Client.State.Navigators;

namespace Client.ViewModels
{
    public class TeamsViewModel : ViewModelBase
    {

        public INavigatorTeam NavigatorTeam { get; set; } = new NavigatorTeam();

        public TeamsViewModel()
        {
            NavigatorTeam.UpdateCurrentViewModelCommand.Execute(TeamViewType.Meetings);
        }

    }
}

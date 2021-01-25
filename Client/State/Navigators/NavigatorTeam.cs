using System.Windows.Input;
using Client.Commands;
using Client.Models;
using Client.ViewModels;
using Client.ViewModels.Teams;

namespace Client.State.Navigators
{
    public class NavigatorTeam : ObservableObject, INavigatorTeam
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand<NavigatorTeam>(this);

        public ViewModelBase GetModelFromTeamViewType(TeamViewType type)
        {
            switch (type)
            {
                case TeamViewType.Results:
                    return new ResultsViewModel();
                case TeamViewType.Create:
                    return new CreateViewModel();
            }

            return null;
        }
    }
}
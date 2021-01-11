using Client.ViewModels;
using System.Windows.Input;
using Client.Commands;
using Client.Models;

namespace Client.State.Navigators
{
    public class Navigator : ObservableObject, INavigator
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

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand<Navigator>(this);

        public ViewModelBase GetModelFromViewType(ViewType type)
        {
            switch (type)
            {
                case ViewType.Home:
                    return new HomeViewModel();

                case ViewType.Teams:
                    return new TeamsViewModel();

                case ViewType.Join:
                    return new JoinTeamModel();

                case ViewType.Calendar:
                    return new CalendarViewModel();
            }

            return null;
        }
    }
}
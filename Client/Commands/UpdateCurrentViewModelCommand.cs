using System;
using System.Windows.Input;
using Client.State.Navigators;
using Client.ViewModels;

namespace Client.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType) parameter;

                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new HomeViewModel();
                        break;
                    case ViewType.Teams:
                        _navigator.CurrentViewModel = new TeamsViewModel();
                        break;
                    case ViewType.Create:
                        _navigator.CurrentViewModel = new JoinTeamModel();
                        break;
                    case ViewType.Calendar:
                        _navigator.CurrentViewModel = new CalendarViewModel();
                        break;
                    case ViewType.Login:
                        _navigator.CurrentViewModel = new LoginViewModel();
                        break;
                    case ViewType.Register:
                        _navigator.CurrentViewModel = new RegisterViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
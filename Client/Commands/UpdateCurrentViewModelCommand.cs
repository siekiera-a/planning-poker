using System;
using System.Windows.Input;
using Client.State.Navigators;
using Client.ViewModels;
using Client.ViewModels.Teams;

namespace Client.Commands
{
    public class UpdateCurrentViewModelCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private T _navigator;

        public UpdateCurrentViewModelCommand(T navigator)
        {
            _navigator = navigator;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_navigator is Navigator obj)
            {
                ViewType viewType = (ViewType) parameter;
                obj.CurrentViewModel = obj.GetModelFromViewType(viewType);
            }
            else if (_navigator is NavigatorTeam obj1)
            {
                TeamViewType viewType = (TeamViewType) parameter;
                obj1.CurrentViewModel = obj1.GetModelFromTeamViewType(viewType);
            }
        }
    }
}
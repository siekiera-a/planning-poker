using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Client.ViewModels;

namespace Client.State.Navigators
{
    public enum TeamViewType
    {
        Meetings,
        Results,
        Create
    }

    public interface INavigatorTeam
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
        ViewModelBase GetModelFromTeamViewType(TeamViewType type);
    }
}

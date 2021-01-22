using System.Windows.Input;
using Client.Commands;
using Client.Commands.JoinTeam;

namespace Client.ViewModels
{
    public class JoinTeamModel : ViewModelBase
    {
        private string _teamName;

        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value;
                OnPropertyChanged(nameof(TeamName));
            }
        }

        private string _newTeamName;

        public string NewTeamName
        {
            get { return _newTeamName; }
            set
            {
                _newTeamName = value;
                OnPropertyChanged(nameof(NewTeamName));
            }
        }

        public ICommand JoinTeamCommand { get; }
        public ICommand CreateTeamCommand { get; }

        public JoinTeamModel()
        {
            JoinTeamCommand = new JoinTeamCommand(this);
            CreateTeamCommand = new CreateTeamCommand(this);
        }
    }
}
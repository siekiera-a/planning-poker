using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Commands;
using Client.Commands.JoinTeam;
using Client.Service;
using Server.Dtos.Outgoing;

namespace Client.ViewModels
{
    public class JoinTeamModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;

        private string _teamName;

        public string TeamName
        {
            get => _teamName;
            set
            {
                _teamName = value;
                OnPropertyChanged(nameof(TeamName));
            }
        }

        private string _newTeamName;

        public string NewTeamName
        {
            get => _newTeamName;
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
            _apiClient = Services.GetService<IApiClient>();
        }

        public string NotificationText { get; set; } = "";

        public async Task FetchDataJoinTeam()
        {
            var response =
                await _apiClient.PostAsyncAuth<TeamIdResponse>("/team/join-code", new {Code = TeamName});

            if (response.IsOk)
            {
                NotificationText = "You've joined the team";
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.BadRequest ||
                    response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    NotificationText = response.Error.Message;
                }
            }

            TeamName = "";
        }

        public async Task FetchDataCreateTeam()
        {
            var response = await _apiClient.PostAsyncAuth<TeamResponse>("/team", new {Name = NewTeamName});

            if (response.IsOk)
            {
                NotificationText = "New team created successfully";
            }
            else
            {
                NotificationText = "Unable to create team";
            }

            NewTeamName = "";
        }
    }
}
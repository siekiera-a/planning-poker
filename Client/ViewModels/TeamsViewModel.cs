using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Service;
using Client.State.Navigators;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;

namespace Client.ViewModels
{
    public class TeamsViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;

        public ObservableCollection<TeamResponse> Teams { get; }

        public ObservableCollection<User> Members { get; }

        private int _teamId;

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        public async Task FetchTeams()
        {
            var response = await _apiClient.GetAsyncAuth<TeamResponse[]>("/team");

            Teams.Clear();

            if (response.IsOk)
            {
                foreach (var teamResponse in response.Value)
                {
                    Teams.Add(teamResponse);
                }
            }
        }

        public async Task FetchMembers(int teamId)
        {
            _teamId = teamId;
            var members = await _apiClient.GetAsyncAuth<GetMembersResponse>($"/team/{_teamId}/members");

            Members.Clear();

            if (members.IsOk)
            {
                foreach (var memberResponse in members.Value.Members)
                {
                    Members.Add(memberResponse);
                }
            }
        }

        public async Task GenerateCode()
        {
            var response = await _apiClient.GetAsyncAuth<CodeResponse>($"/team/{_teamId}/join-code");

            if (response.IsOk)
            {
                Code = response.Value.Code;
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    Code = response.Error.Message;
                }
            }
        }

        public INavigatorTeam NavigatorTeam { get; set; } = new NavigatorTeam();

        public TeamsViewModel()
        {
            NavigatorTeam.UpdateCurrentViewModelCommand.Execute(TeamViewType.Meetings);
            _apiClient = Services.GetService<IApiClient>();
            Teams = new ObservableCollection<TeamResponse>();
            Members = new ObservableCollection<User>();
        }
    }
}
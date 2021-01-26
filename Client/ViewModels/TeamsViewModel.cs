using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Service;
using Client.State.Navigators;
using Server.Dtos.Incoming;
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

        private string _mail;

        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged(nameof(Mail));
            }
        }

        public string NotificationText { get; set; } = "";

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
            Code = "";

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
            if (_teamId != 0)
            {
                var response = await _apiClient.GetAsyncAuth<CodeResponse>($"/team/{_teamId}/join-code");

                if (response.IsOk)
                {
                    Code = response.Value.Code;
                    NotificationText = "Join code created successfully";
                }
                else
                {
                    if (response.HttpStatusCode == HttpStatusCode.Forbidden)
                    {
                        NotificationText = "You don't have access";
                    }
                }
            }
            else
            {
                NotificationText = "Choose team to generate code";
            }
        }

        public async Task DeleteCode()
        {
            if (_teamId != 0)
            {
                var response = await _apiClient.DeleteAsyncAuth<BoolResponse>($"/team/{_teamId}/join-code");

                if (response.IsOk)
                {
                    if (response.Value.Success)
                    {
                        Code = "";
                        NotificationText = "Join code deleted successfully";
                    }
                    else
                    {
                        NotificationText = "Unable to delete join code";
                    }
                }
                else
                {
                    NotificationText = "You don't have access";
                }
            }
            else
            {
                NotificationText = "Choose team to delete code";
            }
        }

        public async Task AddByMail()
        {
            var response =
                await _apiClient.PostAsyncAuth<BoolResponse>($"/team/{_teamId}/members",
                    new AddMemberRequest {Email = Mail});

            if (response.IsOk)
            {
                if (response.Value.Success)
                {
                    NotificationText = "Member added successfully";
                }
                else
                {
                    NotificationText = "Unable to add member";
                }
            }
            else if (response.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                NotificationText = "You don't have access";
            }

            Mail = "";
        }

        public INavigatorTeam NavigatorTeam { get; set; } = new NavigatorTeam();

        public TeamsViewModel()
        {
            NavigatorTeam.UpdateCurrentViewModelCommand.Execute(TeamViewType.Create);
            _apiClient = Services.GetService<IApiClient>();
            Teams = new ObservableCollection<TeamResponse>();
            Members = new ObservableCollection<User>();
        }
    }
}
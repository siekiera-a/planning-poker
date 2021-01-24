using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
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


        public INavigatorTeam NavigatorTeam { get; set; } = new NavigatorTeam();

        public TeamsViewModel()
        {
            NavigatorTeam.UpdateCurrentViewModelCommand.Execute(TeamViewType.Meetings);
            _apiClient = Services.GetService<IApiClient>();
            Teams = new ObservableCollection<TeamResponse>();
        }
    }
}
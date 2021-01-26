using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Models;
using Client.Service;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;

namespace Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;

        public ObservableCollection<UserResultResponse> Tasks { get; }

        public string Username { get; set; }

        public async Task FetchTasks()
        {
            var response = await _apiClient.PostAsyncAuth<List<UserResultResponse>>("/meeting/results",
                new DateTimeRequest {DateTime = DateTime.UtcNow.AddDays(-7)});
            Tasks.Clear();
            if (response.IsOk)
            {
                foreach (var r in response.Value)
                {
                    Tasks.Add(r);
                }
            }
        }

        public HomeViewModel()
        {
            var userData = Services.GetService<IUserDataProvider>();
            _apiClient = Services.GetService<IApiClient>();
            Username = userData.Username;
            Tasks = new ObservableCollection<UserResultResponse>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Client.Service;
using Client.ViewModels.Game;
using Client.Views.Game;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;
using Server.Models.Server;

namespace Client.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private IGameManager _gameManager;
        private readonly IUserDataProvider _userData;
        private DateTime _dateTime = DateTime.Today.ToUniversalTime();
        public ObservableCollection<MeetingDetailsResponse> Meetings { get; }

        public DateTime MyDateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                OnPropertyChanged(nameof(MyDateTime));
            }
        }

        public async Task FetchMeetings()
        {
            var response =
                await _apiClient.PostAsyncAuth<List<MeetingDetailsResponse>>("/meeting",
                    new DateTimeRequest {DateTime = MyDateTime});

            Meetings.Clear();

            if (response.IsOk)
            {
                foreach (var r in response.Value)
                {
                    Meetings.Add(r);
                }
            }
        }

        public async void JoinMeeting(MeetingDetailsResponse meeting)
        {
            if (meeting != null && meeting.CanJoin)
            {
                _gameManager = Services.GetService<IGameManager>();

                var response = await _gameManager.Connect(meeting.Id);

                if (response.Success)
                {
                    if (response.IsOrganizer)
                    {
                        Window window = new AdminGameWindow
                        {
                            DataContext = new AdminGameViewModel()
                        };
                        window.Show();
                    }
                    else
                    {
                        Window window = new GameWindow
                        {
                            DataContext = new UserGameViewModel(meeting.Id)
                        };
                        window.Show();
                    }
                }
            }
        }

        public CalendarViewModel()
        {
            _apiClient = Services.GetService<IApiClient>();
            // _token = Services.GetService<ITokenManager>();
            _userData = Services.GetService<IUserDataProvider>();
            Meetings = new ObservableCollection<MeetingDetailsResponse>();
        }
    }
}
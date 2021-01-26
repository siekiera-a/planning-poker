using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Client.Service;
using Client.Views.Game;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;

namespace Client.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
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

        public void JoinMeeting(MeetingDetailsResponse meeting)
        {
            if (meeting != null && meeting.CanJoin)
            {
                // if ()
                // {
                //     Window window = new AdminGameWindow();
                //     window.Show();
                // }
                // else
                // {
                //     Window window = new GameWindow();
                //     window.Show();
                // }
            }
            
        }

        public CalendarViewModel()
        {
            _apiClient = Services.GetService<IApiClient>();
            _userData = Services.GetService<IUserDataProvider>();
            Meetings = new ObservableCollection<MeetingDetailsResponse>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Service;
using Server.Dtos.Incoming;
using Server.Models.Dapper;

namespace Client.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private DateTime _dateTime = DateTime.Today.ToLocalTime();
        public ObservableCollection<MeetingDetails> Meetings { get; }

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
                await _apiClient.PostAsyncAuth<List<MeetingDetails>>($"/meeting",
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

        public CalendarViewModel()
        {
            _apiClient = Services.GetService<IApiClient>();
            Meetings = new ObservableCollection<MeetingDetails>();
        }
    }
}
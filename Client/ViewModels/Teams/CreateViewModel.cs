using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Client.Service;
using Dapper;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;

namespace Client.ViewModels.Teams
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private string _newTask;
        private int _teamId;
        public ObservableCollection<string> Tasks { get; }
        public ObservableCollection<TeamResponse> Teams { get; }
        public ObservableCollection<User> Members { get; }
        public ObservableCollection<User> SelectedMembers { get; }
        private DateTime _dateTime = DateTime.Today.ToLocalTime();

        public string NotificationText { get; set; } = "";

        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public string NewTask
        {
            get => _newTask;
            set
            {
                _newTask = value;
                OnPropertyChanged(nameof(NewTask));
            }
        }

        public void AddTask()
        {
            if (NewTask.Length > 0 && !Tasks.Contains(NewTask) && NewTask.Trim().Length > 0)
            {
                Tasks.Add(NewTask);
            }

            NewTask = "";
        }

        public void DeleteTask(string taskName)
        {
            if (Tasks.Contains(taskName))
            {
                Tasks.Remove(taskName);
            }
        }

        public async Task FetchTeams()
        {
            var response = await _apiClient.GetAsyncAuth<TeamResponse[]>("/team");

            Teams.Clear();
            Members.Clear();
            SelectedMembers.Clear();

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
            SelectedMembers.Clear();

            if (members.IsOk)
            {
                foreach (var memberResponse in members.Value.Members)
                {
                    Members.Add(memberResponse);
                }
            }
        }

        public void FetchMembersToMeeting(int memberId)
        {
            var member = Members.FirstOrDefault(m => m.Id == memberId);

            if (member != null)
            {
                Members.Remove(member);
                SelectedMembers.Add(member);
            }
        }

        public void DeleteSelectedMembers(IList users)
        {
            var members = users.Cast<User>().AsList();
            foreach (var member in members)
            {
                Members.Add(member);
            }

            var filteredMembers = SelectedMembers.Where(x => !members.Contains(x)).AsList();
            SelectedMembers.Clear();

            foreach (var user in filteredMembers)
            {
                SelectedMembers.Add(user);
            }
        }

        public async Task CreateMeeting()
        {
            if (Tasks.Count > 0 && SelectedMembers.Count > 0)
            {
                var response = await _apiClient.PostAsyncAuth<MeetingIdResponse>($"/meeting/team/{_teamId}",
                    new MeetingRequest
                    {
                        Tasks = Tasks.AsList(), DateTime = DateTime,
                        InvitedUsers = SelectedMembers.Select(x => x.Id).AsList()
                    });

                if (response.IsOk)
                {
                    NotificationText = "Meeting created successfully";
                    ClearFields();
                }
                else if (response.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    NotificationText = "You don't have access";
                }
                else if (response.HttpStatusCode == HttpStatusCode.BadRequest)
                {
                    NotificationText = response.Error.Message;
                }
                else if (response.HttpStatusCode == HttpStatusCode.InternalServerError)
                {
                    NotificationText = "Operation failed";
                }
            }
            else
            {
                NotificationText = "Fill all fields";
            }
        }

        public void ClearFields()
        {
            NewTask = "";
            Tasks.Clear();
            Members.Clear();
            SelectedMembers.Clear();
            DateTime = DateTime.Today.ToLocalTime();
        }

        public CreateViewModel()
        {
            _apiClient = Services.GetService<IApiClient>();
            Tasks = new ObservableCollection<string>();
            Teams = new ObservableCollection<TeamResponse>();
            Members = new ObservableCollection<User>();
            SelectedMembers = new ObservableCollection<User>();
        }
    }
}
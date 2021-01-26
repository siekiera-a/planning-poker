using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Dapper;
using Server.Models.Server;
using Server.Services.Meeting;
using Server.Utils;

namespace Server.Services.Server
{
    public class GroupManager
    {

        public int MeetingId { get; }
        public bool IsFinished => _index >= _tasks.Count;
        public TaskInfo Task => _tasks[_index];
        private readonly IMeetingService _meetingService;
        public List<Client> Clients { get; }
        private readonly List<TaskInfo> _tasks;
        private int _index;

        public GroupManager(int meetingId, IMeetingService meetingService)
        {
            MeetingId = meetingId;
            _meetingService = meetingService;
            Clients = new List<Client>();
            _tasks = new List<TaskInfo>();
        }

        public async Task<bool> Load()
        {
            var tasks = await _meetingService.GetTasksForMeeting(MeetingId);
            _tasks.Clear();
            _tasks.AddRange(tasks);

            if (_tasks.Any())
            {
                var index = tasks.FindIndex(x => x.EstimatedTime == 0);

                if (index < 0)
                {
                    return false;
                }

                _index = index;
                return true;
            }

            return false;
        }

        

        public void Next()
        {
            if (!IsFinished)
            {
                _index++;
            }
        }

        public Optional<short> GetEstimatedTime(int userId)
        {
            var user = Clients.Find(c => c.Id == userId);

            if (user == null)
            {
                return Optional<short>.Empty();
            }

            return Optional<short>.of(user.EstimatedTime);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Server;
using Server.Services.Authentication;
using Server.Services.Authorization;
using Server.Services.Meeting;
using Server.Utils;

namespace Server.Services.Server
{
    public class GameController
    {

        private readonly IDictionary<int, GroupManager> _managers;

        public GameController()
        {
            _managers = new Dictionary<int, GroupManager>();
        }

        public Optional<GroupManager> GetManager(int meetingId)
        {
            lock (_managers)
            {
                if (_managers.ContainsKey(meetingId))
                {
                    return Optional<GroupManager>.of(_managers[meetingId]);
                }
            }

            return Optional<GroupManager>.Empty();
        }

        public async Task<bool> AddGroupManager(int meetingId, GroupManager manager)
        {
            lock (_managers)
            {
                _managers.Add(meetingId, manager);
            }
            return await manager.Load();
        }


        public void RemoveManager(int meetingId)
        {
            lock (_managers)
            {
                _managers.Remove(meetingId);
            }
        }

        public ClientResponse Next(int meetingId)
        {
            var manager = GetManager(meetingId);

            if (manager.IsEmpty)
            {
                return new ClientResponse
                {
                    IsFinished = true,
                    Description = ""
                };
            }

            var mg = manager.Value;
            mg.Next();

            var response = new ClientResponse
            {
                IsFinished = mg.IsFinished,
                Description = ""
            };

            mg.Clients.ForEach(x => x.EstimatedTime = 0);

            if (!mg.IsFinished)
            {
                response.Description = mg.Task.Description;
            }

            return response;
        }

        public bool Submit(int meetingId, int userId, short estimatedTime)
        {
            var manager = GetManager(meetingId);

            var user = manager.Value.Clients.Find(x => x.Id == userId);

            if (user != null)
            {
                user.EstimatedTime = estimatedTime;
                return true;
            }

            return false;
        }


        public OrganizerResponse GetResponse(int meetingId)
        {
            // add authorization
            var manager = GetManager(meetingId);
            return new OrganizerResponse
            {
                Clients = manager.Value.Clients.FindAll(x => x.Id != manager.Value.OrganizerId),
                IsFinished = manager.Value.IsFinished,
                Description = manager.Value.Task.Description
            };
        }

    }
}

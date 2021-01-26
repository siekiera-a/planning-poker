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
        private readonly IMeetingService _meetingService;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IUserProvider _userProvider;

        public GameController(IMeetingService meetingService, IUserAuthorization userAuthorization, IUserProvider userProvider)
        {
            _meetingService = meetingService;
            _userAuthorization = userAuthorization;
            _userProvider = userProvider;
            _managers = new Dictionary<int, GroupManager>();
        }

        public GroupManager GetManager(int meetingId)
        {
            GroupManager manager;

            lock (_managers)
            {
                manager = _managers[meetingId];
            }

            return manager;
        }

        public async Task<Optional<GroupManager>> GetOrCreateGroupManager(int meetingId)
        {
            lock (_managers)
            {
                if (_managers.ContainsKey(meetingId))
                {
                    return Optional<GroupManager>.of(_managers[meetingId]);
                }
            }

            var manager = new GroupManager(meetingId, _meetingService);

            lock (_managers)
            {
                _managers.Add(meetingId, manager);
            }

            var success = await manager.Load();

            return success ? Optional<GroupManager>.of(manager) : Optional<GroupManager>.Empty();
        }

        public async Task<Models.Dapper.Meeting> EndMeeting(int meetingId)
        {
            lock (_managers)
            {
                _managers.Remove(meetingId);
            }

            var success = await _meetingService.EndMeeting(meetingId);

            if (success.IsPresent)
            {
                return success.Value;
            }

            return null;
        }

        public async Task<bool> AssignUser(int meetingId, int userId)
        {
            var hasPermission =
                await _userAuthorization.Authorize(_userProvider.GetUserId(), meetingId, MeetingAction.AssignUser);

            if (hasPermission)
            {
                var manager = GetManager(meetingId);

                var estimatedTime = manager.GetEstimatedTime(userId);

                if (estimatedTime.IsPresent)
                {
                    return await _meetingService.AssignUserToTask(meetingId, userId, manager.Task.Id, estimatedTime.Value);
                }
            }

            return false;
        }

        public async Task<bool> CanRewind(int meetingId)
        {
            return await _userAuthorization.Authorize(_userProvider.GetUserId(), meetingId, MeetingAction.Rewind);
        }

        public async Task<bool> CanChangeTask(int meetingId)
        {
            return await _userAuthorization.Authorize(_userProvider.GetUserId(), meetingId, MeetingAction.NextTask);
        }

        public async Task<ClientResponse> Next(int meetingId)
        {
            var manager = GetManager(meetingId);

            manager.Next();

            var response = new ClientResponse
            {
                IsFinished = manager.IsFinished,
                Description = ""
            };

            if (!manager.IsFinished)
            {
                response.Description = manager.Task.Description;
            }

            return response;
        }

        public async Task<bool> Submit(int meetingId, short estimatedTime)
        {
            GroupManager manager = GetManager(meetingId);

            var id = _userProvider.GetUserId();

            var user = manager.Clients.Find(x => x.Id == id);

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
                Clients = manager.Clients,
                IsFinished = manager.IsFinished,
                Description = manager.Task.Description
            };
        }

    }
}

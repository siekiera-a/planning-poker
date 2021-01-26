using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Server.DAOs;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;
using Server.Services.Authentication;
using Server.Services.Authorization;
using Server.Services.Team;
using Server.Utils;

namespace Server.Services.Meeting
{
    public class MeetingService : IMeetingService
    {

        private readonly IUserAuthorization _userAuthorization;
        private readonly MeetingDAO _meetingDao;
        private readonly InvitationDAO _invitationDao;
        private readonly TaskDAO _taskDao;
        private readonly ResultDAO _resultDao;
        private readonly int _userId;

        public MeetingService(IUserAuthorization userAuthorization, IUserProvider userProvider, MeetingDAO meetingDao, InvitationDAO invitationDao, TaskDAO taskDao, ResultDAO resultDao)
        {
            _userAuthorization = userAuthorization;
            _meetingDao = meetingDao;
            _invitationDao = invitationDao;
            _taskDao = taskDao;
            _resultDao = resultDao;
            _userId = userProvider.GetUserId();
        }

        public async Task<Optional<int>> CreateMeeting(DateTime startTime, int teamId)
        {
            var currentTimeUtc = DateTime.UtcNow;

            if (currentTimeUtc.CompareTo(startTime) > 0)
            {
                throw new InvalidOperationException("Start time must be greater or equal current time UTC!");
            }

            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, TeamAction.CreateMeeting);

            if (hasPermissions)
            {
                return await _meetingDao.CreateMeeting(startTime.ToUniversalTime(), teamId, _userId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<Optional<int>> CreateMeeting(int teamId)
        {
            return await CreateMeeting(DateTime.UtcNow, teamId);
        }


        public async Task<Optional<Models.Dapper.Meeting>> EndMeeting(int meetingId)
        {
            return await _meetingDao.EndMeeting(meetingId);
        }

        public Task<bool> RemoveMeeting(int meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RescheduleMeeting(int meetingId, DateTime newStartTime)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MeetingDetailsResponse>> GetMeetings(DateTime date)
        {
            var result = await _meetingDao.GetMeetingsOnTheGivenDay(_userId, date);
            return result.Select(x =>
            {

                DateTime now = DateTime.UtcNow;

                bool canJoin = false;

                if (x.EndTime == new DateTime())
                {
                    if (now >= x.StartTime)
                    {
                        canJoin = true;
                    }
                }

                return new MeetingDetailsResponse
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    OrganizerId = x.OrganizerId,
                    OrganizerName = x.OrganizerName,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    CanJoin = canJoin
                };
            }).AsList();
        }

        public async Task<bool> InviteUser(int meetingId, int userId)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, meetingId, MeetingAction.InviteUser);

            if (hasPermissions)
            {
                return await _invitationDao.InviteUser(meetingId, userId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<bool> RemoveInvitation(int meetingId, int userId)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, meetingId, MeetingAction.RemoveInvitation);

            if (hasPermissions)
            {
                return await _invitationDao.RemoveInvitation(meetingId, userId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<int> AddAllTasks(int meetingId, List<string> tasks)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, meetingId, MeetingAction.AddTask);

            if (hasPermissions)
            {
                return await _taskDao.AddAllTasks(meetingId, tasks);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<int> InviteAllUsers(int meetingId, List<int> users)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, meetingId, MeetingAction.InviteUser);

            if (hasPermissions)
            {
                var usersWithOrganizer = new HashSet<int>(users);
                usersWithOrganizer.Add(_userId);
                return await _invitationDao.InviteAllUsers(meetingId, usersWithOrganizer.AsList());
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<bool> AssignUserToTask(int meetingId, int userId, int taskId, short estimatedTime)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, meetingId, MeetingAction.AssignUser);

            if (hasPermissions)
            {
                return await _resultDao.AssignUserToTask(userId, taskId, estimatedTime);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<List<UserResultResponse>> GetResults(DateTime from)
        {
            var response = await _resultDao.GetResults(_userId, from);

            return response.Select(x => new UserResultResponse
            {
                Description = x.Description,
                EstimatedTime = x.EstimatedTime,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                TeamName = x.TeamName,
                TeamId = x.TeamId,
                IsFinished = x.EndTime != new DateTime()
            }).AsList();
        }

        public async Task<List<TaskInfo>> GetTasksForMeeting(int meetingId)
        {
            return await _taskDao.GetTasksForMeeting(meetingId);
        }

        public async Task<Optional<Permissions>> JoinMeetingPermission(int meetingId)
        {
            return await _meetingDao.JoinMeetingPermission(_userId, meetingId);
        }
    }
}

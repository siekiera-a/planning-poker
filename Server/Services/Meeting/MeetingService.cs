using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DAOs;
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
        private readonly int _userId;

        public MeetingService(IUserAuthorization userAuthorization, IUserProvider userProvider, MeetingDAO meetingDao)
        {
            _userAuthorization = userAuthorization;
            _meetingDao = meetingDao;
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
                return await _meetingDao.CreateMeeting(startTime, teamId, _userId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<Optional<int>> CreateMeeting(int teamId)
        {
            return await CreateMeeting(DateTime.UtcNow, teamId);
        }


        public Task<Optional<Models.Dapper.Meeting>> EndMeeting(int meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMeeting(int meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RescheduleMeeting(int meetingId, DateTime newStartTime)
        {
            throw new NotImplementedException();
        }

        public Task<List<MeetingDetails>> GetFutureMeetings()
        {
            throw new NotImplementedException();
        }

        public Task<bool> InviteUser(int meetingId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveInvitation(int meetingId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using System;
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
        private readonly InvitationDAO _invitationDao;
        private readonly int _userId;

        public MeetingService(IUserAuthorization userAuthorization, IUserProvider userProvider, MeetingDAO meetingDao, InvitationDAO invitationDao)
        {
            _userAuthorization = userAuthorization;
            _meetingDao = meetingDao;
            _invitationDao = invitationDao;
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

        public async Task<List<MeetingDetails>> GetFutureMeetings()
        {
            return await _meetingDao.GetFutureMeetings(_userId);
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
    }
}

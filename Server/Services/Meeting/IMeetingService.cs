﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;
using Server.Utils;

namespace Server.Services.Meeting
{
    public interface IMeetingService
    {

        Task<Optional<int>> CreateMeeting(DateTime startTime, int teamId);
        Task<Optional<int>> CreateMeeting(int teamId);
        Task<Optional<Models.Dapper.Meeting>> EndMeeting(int meetingId);
        Task<bool> RemoveMeeting(int meetingId);
        Task<bool> RescheduleMeeting(int meetingId, DateTime newStartTime);
        Task<List<MeetingDetailsResponse>> GetMeetings(DateTime date);
        Task<bool> InviteUser(int meetingId, int userId);
        Task<bool> RemoveInvitation(int meetingId, int userId);

        // it should not look like that !!!!
        Task<int> AddAllTasks(int meetingId, List<string> tasks);
        Task<int> InviteAllUsers(int meetingId, List<int> users);
        Task<bool> AssignUserToTask(int meetingId, int userId, int taskId, Int16 estimatedTime);
        Task<List<UserResultResponse>> GetResults(DateTime from);
        Task<List<TaskInfo>> GetTasksForMeeting(int meetingId);
        Task<Optional<Permissions>> JoinMeetingPermission(int meetingId);

    }
}

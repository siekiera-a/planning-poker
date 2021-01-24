using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        Task<List<MeetingDetails>> GetFutureMeetings();
        Task<bool> InviteUser(int meetingId, int userId);
        Task<bool> RemoveInvitation(int meetingId, int userId);

    }
}

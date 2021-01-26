using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Services.Meeting;
using Server.Services.Team;

namespace Server.Services.Authorization
{
    public interface IUserAuthorization
    {

        public Task<bool> Authorize(int userId, int teamId, TeamAction action);
        public Task<bool> Authorize(int userId, int meetingId, MeetingAction action);

    }
}

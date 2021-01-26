using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Models.Server;
using Server.Services.Authentication;
using Server.Services.Meeting;

namespace Server.Services.Server
{
    [Authorize]
    public class Game : Hub
    {

        private readonly IMeetingService _meetingService;

        public Game(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public async Task<JoinResponse> Join(int meetingId)
        {
            var permissions = await _meetingService.JoinMeetingPermission(meetingId);

            if (permissions.IsEmpty)
            {
                return new JoinResponse
                {
                    IsOrganizer = false,
                    Success = false
                };
            }

            var p = permissions.Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId.ToString());
            return new JoinResponse { Success = true, IsOrganizer = p.IsOrganizer };
        }

    }
}

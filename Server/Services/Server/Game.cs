using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Models.Server;
using Server.Services.Authentication;
using Server.Services.Authorization;
using Server.Services.Meeting;

namespace Server.Services.Server
{
    [Authorize]
    public class Game : Hub
    {

        private readonly IMeetingService _meetingService;
        private readonly IUserProvider _userProvider;

        private readonly GameController _controller;

        public Game(IMeetingService meetingService, GameController controller, IUserProvider userProvider)
        {
            _meetingService = meetingService;
            _controller = controller;
            _userProvider = userProvider;
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

            var manager = await _controller.GetOrCreateGroupManager(meetingId);

            if (manager.IsEmpty)
            {
                return new JoinResponse
                {
                    IsOrganizer = false,
                    Success = false
                };
            }

            manager.Value.Clients.Add(new Client { Email = p.Email, Id = _userProvider.GetUserId() });
            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId.ToString());
            return new JoinResponse { Success = true, IsOrganizer = p.IsOrganizer };
        }

        public async Task<bool> AssignUser(int meetingId, int userId)
        {
            return await _controller.AssignUser(meetingId, userId);
        }

        public async Task Rewind(int meetingId)
        {
            if (await _controller.CanRewind(meetingId))
            {
                var manager = await _controller.GetOrCreateGroupManager(meetingId);

                if (manager.IsPresent)
                {
                    var task = manager.Value.Task;
                    await Clients.Group(meetingId.ToString()).SendAsync("TaskChange",
                        new ClientResponse { Description = task.Description, IsFinished = manager.Value.IsFinished });
                    await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
                }

            }
        }

        public async Task Next(int meetingId)
        {
            if (await _controller.CanChangeTask(meetingId))
            {
                var response = await _controller.Next(meetingId);
                if (response.IsFinished)
                {
                    await _controller.EndMeeting(meetingId);
                    await Clients.Group(meetingId.ToString()).SendAsync("CloseWindow");
                }
                else
                {
                    await Clients.Group(meetingId.ToString()).SendAsync("TaskChange", response);
                    await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
                }
            }
        }

        public async Task Submit(int meetingId, ClientRequest request)
        {
            if (await _controller.Submit(meetingId, request.EstimatedTime))
            {
                await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
            }
        }


    }
}

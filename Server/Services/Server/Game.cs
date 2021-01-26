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
        private readonly IUserAuthorization _userAuthorization;

        private readonly GameController _controller;

        public Game(IMeetingService meetingService, GameController controller, IUserProvider userProvider, IUserAuthorization userAuthorization)
        {
            _meetingService = meetingService;
            _controller = controller;
            _userProvider = userProvider;
            _userAuthorization = userAuthorization;
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

            var mg = _controller.GetManager(meetingId);
            GroupManager manager;

            if (mg.IsEmpty)
            {
                manager = new GroupManager(meetingId, _meetingService);
                if (!(await _controller.AddGroupManager(meetingId, manager)))
                {
                    return new JoinResponse
                    {
                        IsOrganizer = false,
                        Success = false
                    };
                }
            }
            else
            {
                manager = mg.Value;
            }

            if (p.IsOrganizer)
            {
                manager.OrganizerId = _userProvider.GetUserId();
            }

            manager.Clients.Add(new Client { Email = p.Email, Id = _userProvider.GetUserId() });
            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId.ToString());

            await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));

            return new JoinResponse { Success = true, IsOrganizer = p.IsOrganizer };
        }

        public async Task<bool> AssignUser(int meetingId, int userId)
        {
            var manager = _controller.GetManager(meetingId);

            if (manager.IsPresent)
            {
                var estimatedTime = manager.Value.GetEstimatedTime(userId);

                if (estimatedTime.IsPresent)
                {
                    return await _meetingService.AssignUserToTask(meetingId, userId, manager.Value.Task.Id,
                        estimatedTime.Value);
                }
            }

            return false;
        }

        public async Task Rewind(int meetingId)
        {
            var manager = _controller.GetManager(meetingId);

            if (manager.IsPresent)
            {
                var task = manager.Value.Task;
                manager.Value.Clients.ForEach(x => x.EstimatedTime = 0);
                await Clients.Group(meetingId.ToString()).SendAsync("TaskChange",
                    new ClientResponse { Description = task.Description, IsFinished = manager.Value.IsFinished });
                await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
            }
        }

        public async Task Next(int meetingId)
        {
            var response = _controller.Next(meetingId);
            if (response.IsFinished)
            {
                _controller.RemoveManager(meetingId);
                await _meetingService.EndMeeting(meetingId);
                await Clients.Group(meetingId.ToString()).SendAsync("CloseWindow");
            }
            else
            {
                await Clients.Group(meetingId.ToString()).SendAsync("TaskChange", response);
                await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
            }
        }

        public async Task Submit(int meetingId, ClientRequest request)
        {
            if (_controller.Submit(meetingId, _userProvider.GetUserId(), request.EstimatedTime))
            {
                await Clients.Group(meetingId.ToString()).SendAsync("Submitted", _controller.GetResponse(meetingId));
            }
            
        }

        public ClientResponse CurrentTask(int meetingId)
        {
            var manager = _controller.GetManager(meetingId);

            if (manager.IsEmpty)
            {
                new ClientResponse
                {
                    IsFinished = true,
                    Description = ""
                };
            }

            var task = manager.Value.Task;

            return new ClientResponse
            {
                IsFinished = manager.Value.IsFinished,
                Description = task.Description
            };
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Services.Meeting;
using Server.Utils;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {

        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost("team/{id}")]
        public async Task<IActionResult> CreateMeeting(int id, MeetingRequest request)
        {
            try
            {
                Optional<int> meetingId;

                if (request.DateTime != new DateTime())
                {
                    meetingId = await _meetingService.CreateMeeting(request.DateTime, id);
                }
                else
                {
                    meetingId = await _meetingService.CreateMeeting(id);
                }


                if (meetingId.IsEmpty)
                {
                    return StatusCode(500);
                }

                int meeting = meetingId.Value;

                int rowsAffected = await _meetingService.AddAllTasks(meeting, request.Tasks);

                if (rowsAffected < 0)
                {
                    return StatusCode(500);
                }

                rowsAffected = await _meetingService.InviteAllUsers(meeting, request.InvitedUsers);

                if (rowsAffected < 0)
                {
                    return StatusCode(500);
                }

                return Ok(new MeetingIdResponse { MeetingId = meeting });

            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new ErrorResponse { Message = e.Message });
            }
        }

        [HttpPost("{id}/invite")]
        public async Task<IActionResult> InviteUser(int id, UserIdRequest request)
        {
            try
            {
                var result = await _meetingService.InviteUser(id, request.UserId);
                return Ok(new BoolResponse { Success = result });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{meetingId}/invite/{userId}")]
        public async Task<IActionResult> RemoveInvitation(int meetingId, int userId)
        {
            try
            {
                var result = await _meetingService.RemoveInvitation(meetingId, userId);
                return Ok(new BoolResponse { Success = result });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> GetMeetings(DateTimeRequest request)
        {
            var meetings = await _meetingService.GetMeetings(request.DateTime);
            return Ok(meetings); // List<MeetingDetails>
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignUserToTask(int id, AssignRquest request)
        {
            try
            {
                var success = await _meetingService.AssignUserToTask(id, request.UserId, request.TaskId, request.EstimatedTime);
                return Ok(new BoolResponse { Success = success });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

    }
}

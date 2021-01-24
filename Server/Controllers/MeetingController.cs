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
        public async Task<IActionResult> CreateMeeting(int id, DateTimeRequest request)
        {
            try
            {
                var meetingId = await _meetingService.CreateMeeting(request.StartTime, id);

                if (meetingId.IsEmpty)
                {
                    return StatusCode(500);
                }

                return Ok(new MeetingIdResponse { MeetingId = meetingId.Value });

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

        [HttpPost("team/{id}/now")]
        public async Task<IActionResult> CreateMeeting(int id)
        {
            return await CreateMeeting(id, new DateTimeRequest { StartTime = DateTime.UtcNow.AddSeconds(10) });
        }
    }
}

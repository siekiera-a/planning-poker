using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Services.Team;

namespace Server.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _teamService.GetTeams();
            return Ok(teams); // List<TeamResponse>
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTeam(TeamNameRequest request)
        {
            var team = await _teamService.CreateTeam(request.Name);

            if (team.IsEmpty)
            {
                return StatusCode(500);
            }

            return Ok(new TeamResponse { Id = team.Value.Id, Name = team.Value.Name });
        }

        [HttpPost("{id}/change-name")]
        public async Task<IActionResult> ChangeName(int id, TeamNameRequest request)
        {
            try
            {
                var response = await _teamService.ChangeName(id, request.Name);
                return Ok(new BoolResponse { Success = response });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id}/join-code")]
        public async Task<IActionResult> GetJoinCode(int id)
        {
            try
            {
                var response = await _teamService.GenerateJoinCode(id);

                if (response.IsEmpty)
                {
                    return StatusCode(500);
                }

                return Ok(new CodeResponse { TeamId = id, Code = response.Value });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("join-code")]
        public async Task<IActionResult> JoinWithCode(CodeRequest request)
        {
            if (request.Code != null)
            {
                var teamId = await _teamService.JoinWithCode(request.Code);

                if (teamId.IsEmpty)
                {
                    return NotFound(new ErrorResponse { Message = "Could not join to team!" });
                }

                return Ok(new TeamIdResponse { TeamId = teamId.Value });
            }

            return BadRequest(new ErrorResponse { Message = "Code can not be empty!" });
        }

        [HttpDelete("{id}/join-code")]
        public async Task<IActionResult> RemoveJoinCode(int id)
        {
            try
            {
                var response = await _teamService.RemoveJoinCode(id);
                return Ok(new BoolResponse { Success = response });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetMembers(int id)
        {
            try
            {
                var members = await _teamService.GetMembers(id);
                return Ok(new GetMembersResponse { TeamId = id, Members = members });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

        }

        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(int id, AddMemberRequest request)
        {
            try
            {
                var result = await _teamService.AddMember(id, request.Email);
                return Ok(new BoolResponse { Success = result });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}/members/{userId}")]
        public async Task<IActionResult> RemoveMember(int id, int userId)
        {
            try
            {
                var result = await _teamService.RemoveMember(id, userId);
                return Ok(new BoolResponse { Success = result });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{id}/role")]
        public async Task<IActionResult> ChangeRole(int id, ChangeRoleRequest request)
        {
            try
            {
                var result = await _teamService.ChangeUserRole(id, request.UserId, request.Role);
                return Ok(new BoolResponse { Success = result });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}

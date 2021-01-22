using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos.Incoming;
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
            return Ok(new { Teams = teams });
        }

        [HttpGet("members")]
        public async Task<IActionResult> GetMembers(IdRequest teamId)
        {
            var members = await _teamService.GetMembers(teamId.Id);
            return Ok(new { TeamId = teamId, Members = members });
        }

        [HttpPost("change-name")]
        public async Task<IActionResult> ChangeName(TeamRequest team)
        {
            var response = await _teamService.ChangeName(team.Id, team.Name);

            return Ok(new { Success = response });
        }

        [HttpDelete("join-code")]
        public async Task<IActionResult> RemoveJoinCode(IdRequest teamId)
        {
            var response = await _teamService.RemoveJoinCode(teamId.Id);
            return Ok(new { Success = response });
        }

        [HttpGet("join-code")]
        public async Task<IActionResult> GetJoinCode(IdRequest teamId)
        {
            var response = await _teamService.GenerateJoinCode(teamId.Id);

            if (response.IsEmpty)
            {
                return StatusCode(500);
            }

            return Ok(new { TeamId = teamId.Id, Code = response.Value });
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTeam(TeamNameRequest name)
        {
            var team = await _teamService.CreateTeam(name.Name);

            if (team.IsEmpty)
            {
                return StatusCode(500);
            }

            return Ok(team.Value);
        }

    }
}

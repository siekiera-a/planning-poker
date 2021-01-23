using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DAOs;
using Server.Models.Dapper;
using Server.Services.Authentication;
using Server.Utils;

namespace Server.Services.Team
{
    public class TeamService : ITeamService
    {

        private readonly TeamDAO _teamDao;
        private readonly TeamMemberDAO _teamMemberDao;
        private readonly IUserProvider _userProvider;

        public TeamService(IUserProvider userProvider, TeamDAO teamDao, TeamMemberDAO teamMemberDao)
        {
            _userProvider = userProvider;
            _teamDao = teamDao;
            _teamMemberDao = teamMemberDao;
        }

        public async Task<List<TeamBase>> GetTeams()
        {
            int userId = _userProvider.GetUserId();
            return await _teamDao.GetTeams(userId);
        }

        public async Task<bool> ChangeName(int teamId, string newName)
        {
            var result = await _teamDao.ChangeName(teamId, newName);
            return result;
        }

        public async Task<Optional<string>> GenerateJoinCode(int teamId)
        {
            var code = await _teamDao.GenerateJoinCode(teamId);
            return code;
        }

        public async Task<bool> RemoveJoinCode(int teamId)
        {
            var result = await _teamDao.RemoveJoinCode(teamId);
            return result;
        }

        public async Task<Optional<TeamBase>> CreateTeam(string name)
        {
            int userId = _userProvider.GetUserId();
            var teamId = await _teamDao.CreateTeam(name, userId);

            if (teamId.IsEmpty)
            {
                return Optional<TeamBase>.Empty();
            }

            return Optional<TeamBase>.of(new TeamBase()
            {
                Id = teamId.Value,
                Name = name
            });
        }

        public async Task<List<User>> GetMembers(int teamId)
        {
            var members = await _teamDao.GetMembers(teamId);
            return members;
        }

        public async Task<bool> AddMember(int teamId, string email)
        {
            var response = await _teamMemberDao.AddMember(teamId, email);
            return response;
        }

        public async Task<bool> RemoveMember(int teamId, int userId)
        {
            var response = await _teamMemberDao.RemoveMember(teamId, userId);
            return response;
        }

        public async Task<Optional<int>> JoinWithCode(string code)
        {
            int userId = _userProvider.GetUserId();
            var response = await _teamMemberDao.JoinWithCode(userId, code);
            return response;
        }

        public async Task<bool> ChangeUserRole(int teamId, int userId, int role)
        {
            var response = await _teamMemberDao.ChangeUserRole(teamId, userId, role);
            return response;
        }
    }
}

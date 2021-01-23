using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DAOs;
using Server.Models.Dapper;
using Server.Services.Authentication;
using Server.Services.Authorization;
using Server.Utils;

namespace Server.Services.Team
{
    public class TeamService : ITeamService
    {

        private readonly TeamDAO _teamDao;
        private readonly TeamMemberDAO _teamMemberDao;
        private readonly IUserAuthorization _userAuthorization;
        private readonly int _userId;

        public TeamService(IUserProvider userProvider, TeamDAO teamDao, TeamMemberDAO teamMemberDao, IUserAuthorization userAuthorization)
        {
            _teamDao = teamDao;
            _teamMemberDao = teamMemberDao;
            _userAuthorization = userAuthorization;
            _userId = userProvider.GetUserId();
        }

        public async Task<List<TeamBase>> GetTeams()
        {
            return await _teamDao.GetTeams(_userId);
        }

        public async Task<bool> ChangeName(int teamId, string newName)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.RenameTeam);

            if (hasPermissions)
            {
                return await _teamDao.ChangeName(teamId, newName);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<Optional<string>> GenerateJoinCode(int teamId)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.GenerateJoinCode);

            if (hasPermissions)
            {
                return await _teamDao.GenerateJoinCode(teamId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<bool> RemoveJoinCode(int teamId)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.RemoveJoinCode);

            if (hasPermissions)
            {
                return await _teamDao.RemoveJoinCode(teamId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<Optional<TeamBase>> CreateTeam(string name)
        {
            var teamId = await _teamDao.CreateTeam(name, _userId);

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
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.MemberAccess);

            if (hasPermissions)
            {
                return await _teamDao.GetMembers(teamId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<bool> AddMember(int teamId, string email)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.AddMember);

            if (hasPermissions)
            {
                return await _teamMemberDao.AddMember(teamId, email);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<bool> RemoveMember(int teamId, int userId)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.RemoveMember);

            if (hasPermissions)
            {
                return await _teamMemberDao.RemoveMember(teamId, userId);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<Optional<int>> JoinWithCode(string code)
        {
            return await _teamMemberDao.JoinWithCode(_userId, code);
        }

        public async Task<bool> ChangeUserRole(int teamId, int userId, int role)
        {
            var hasPermissions = await _userAuthorization.Authorize(_userId, teamId, Action.ChangeRole);

            if (hasPermissions)
            {
                return await _teamMemberDao.ChangeUserRole(teamId, userId, role);
            }

            throw new UnauthorizedAccessException();
        }
    }
}

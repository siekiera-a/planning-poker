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
        private readonly IUserProvider _userProvider;

        public TeamService(IUserProvider userProvider, TeamDAO teamDao)
        {
            _userProvider = userProvider;
            _teamDao = teamDao;
        }

        public async Task<List<TeamBase>> GetTeams()
        {
            int userId = _userProvider.GetUserId();
            return await _teamDao.GetTeams(userId);
        }

        public Task<bool> ChangeName(int TeamId, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<Optional<string>> GenerateJoinCode(int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveJoinCode(int teamId)
        {
            throw new NotImplementedException();
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

        public Task<List<User>> GetMembers(int teamId)
        {
            throw new NotImplementedException();
        }
    }
}

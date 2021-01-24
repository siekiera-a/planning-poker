using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Models.Dapper;
using Server.Services.DataAccess;
using Server.Utils;

namespace Server.DAOs
{
    public class TeamDAO : BaseDAO
    {
        private readonly string _prefix = "dbo.spTeam";
        private readonly ILogger<TeamDAO> _logger;

        public TeamDAO(IConnectionFactory connectionFactory, ILogger<TeamDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task<List<TeamBase>> GetTeams(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var teams = await connection.QueryAsync<TeamBase>("SELECT * FROM dbo.ufnGetTeams(@UserId)",
                    new { UserId = userId }, commandType: CommandType.Text);
                return teams.AsList();
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return new List<TeamBase>();
            }
        }

        public async Task<List<User>> GetMembers(int teamId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var members = await connection.QueryAsync<User>("SELECT * FROM dbo.ufnGetTeamMembers(@TeamId)",
                    new { TeamId = teamId }, commandType: CommandType.Text);
                return members.AsList();
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return new List<User>();
            }
        }

        public async Task<Optional<int>> CreateTeam(string name, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var teamId = await connection.QueryFirstAsync<int>($"{_prefix}_CreateTeam", new { Name = name, UserId = userId },
                    commandType: CommandType.StoredProcedure);

                return Optional<int>.of(teamId);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return Optional<int>.Empty();
            }
        }

        public async Task<Optional<string>> GenerateJoinCode(int teamId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var code = await connection.QueryFirstAsync<string>($"{_prefix}_GenerateJoinCode", new { Id = teamId },
                    commandType: CommandType.StoredProcedure);

                return Optional<string>.of(code);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return Optional<string>.Empty();
            }
        }

        public async Task<bool> RemoveJoinCode(int teamId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                int rowsAffected = await connection.ExecuteAsync($"{_prefix}_RemoveJoinCode", new { Id = teamId },
                     commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> ChangeName(int teamId, string newName)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync($"{_prefix}_ChangeName", new { Id = teamId, NewName = newName },
                    commandType: CommandType.StoredProcedure);

                return true;
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

    }
}

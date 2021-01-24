using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Services.DataAccess;
using Server.Utils;

namespace Server.DAOs
{
    public class TeamMemberDAO : BaseDAO
    {

        private readonly string _prefix = "dbo.spTeamMember";
        private readonly ILogger<TeamMemberDAO> _logger;

        public TeamMemberDAO(IConnectionFactory connectionFactory, ILogger<TeamMemberDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }


        public async Task<bool> AddMember(int teamId, string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync($"{_prefix}_AddMember", new { TeamId = teamId, Email = email },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<Optional<int>> JoinWithCode(int userId, string code)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var teamId = await connection.QueryFirstOrDefaultAsync<int>($"{_prefix}_JoinWithCode", new { UserId = userId, Code = code },
                    commandType: CommandType.StoredProcedure);
                return teamId == 0 ? Optional<int>.Empty() : Optional<int>.of(teamId);
            }
            catch (SqlException)
            {
                return Optional<int>.Empty();
            }
        }

        public async Task<bool> RemoveMember(int teamId, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync($"{_prefix}_RemoveMember", new { TeamId = teamId, UserId = userId },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> ChangeUserRole(int teamId, int userId, int role)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync($"{_prefix}_ChangeUserRole", new { TeamId = teamId, UserId = userId, Role = role },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

    }
}

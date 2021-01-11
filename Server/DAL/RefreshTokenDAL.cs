using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Services.DataAccess;

namespace Server.DAL
{
    public class RefreshTokenDAL : BaseDAL
    {

        private readonly ILogger<RefreshTokenDAL> _logger;
        private readonly string _prefix = "dbo.spRefreshToken";

        public RefreshTokenDAL(IConnectionFactory connectionFactory, ILogger<RefreshTokenDAL> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task Logout(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                await connection.ExecuteAsync($"{_prefix}_Logout", new { UserId = id }, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
            }
        }

        /// <summary>
        /// Create refresh token
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="token">max length (50) random string</param>
        /// <returns>true if successfully created; otherwise false</returns>
        public async Task<bool> CreateToken(int userId, string token)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                await connection.ExecuteAsync($"{_prefix}_CreateToken", new { UserId = userId, Token = token },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        private async Task RemoveExpiredTokens()
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                await connection.ExecuteAsync($"{_prefix}_RemoveExpiredTokens",
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
            }
        }

    }
}

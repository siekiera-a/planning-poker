using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Services.DataAccess;

namespace Server.DAOs
{
    public class ResultDAO : BaseDAO
    {

        private readonly string _prefix = "dbo.spResult";
        private readonly ILogger<ResultDAO> _logger;

        public ResultDAO(IConnectionFactory connectionFactory, ILogger<ResultDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task<bool> AssignUserToTask(int userId, int taskId, Int16 estimatedTime)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                await connection.ExecuteAsync($"{_prefix}_AssignUserToTask",
                    new { UserId = userId, TaskId = taskId, EstimatedTime = estimatedTime },
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

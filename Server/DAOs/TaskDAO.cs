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
    public class TaskDAO : BaseDAO
    {

        private readonly string _prefix = "dbo.spTask";
        private readonly ILogger<TaskDAO> _logger;

        public TaskDAO(IConnectionFactory connectionFactory, ILogger<TaskDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task<Optional<int>> AddTask(string description, int meetingId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var id = await connection.QueryFirstOrDefaultAsync<int>($"{_prefix}_AddTask", new { Description = description, MeetingId = meetingId },
                    commandType: CommandType.StoredProcedure);
                return id == 0 ? Optional<int>.Empty() : Optional<int>.of(id);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return Optional<int>.Empty();
            }
        }

        public async Task<bool> EditTask(int taskId, string newDescription)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                var rowsAffected = await connection.ExecuteAsync($"{_prefix}_EditTask", new { Id = taskId, NewDescription = newDescription },
                    commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveTask(int taskId)
        {
            using var connection = _connectionFactory.CreateConnection();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<bool>($"{_prefix}_RemoveTask", new {Id = taskId},
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

    }
}

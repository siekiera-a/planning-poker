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
    public class InvitationDAO : BaseDAO
    {

        private readonly ILogger<InvitationDAO> _logger;
        private readonly string _prefix = "dbo.spInvitation";

        public InvitationDAO(IConnectionFactory connectionFactory, ILogger<InvitationDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task<bool> InviteUser(int meetingId, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                return await connection.QueryFirstOrDefaultAsync<bool>($"{_prefix}_InviteUser",
                    new { MeetingId = meetingId, UserId = userId },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveInvitation(int meetingId, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                await connection.ExecuteAsync($"{_prefix}_RemoveInvitation",
                    new { MeetingId = meetingId, UserId = userId },
                    commandType: CommandType.StoredProcedure);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<int> InviteAllUsers(int meetingId, List<int> users)
        {
            try
            {
                var tasks = new List<Task<bool>>();
                foreach (var user in users)
                {
                    tasks.Add(InviteUser(meetingId, user));
                }

                await Task.WhenAll(tasks);
                return tasks.Count;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return -1;
            }
        }

    }
}

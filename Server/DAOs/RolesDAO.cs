using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Server.Services.DataAccess;

namespace Server.DAOs
{
    public class RolesDAO : BaseDAO
    {
        public RolesDAO(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<List<string>> GetRoles(int userId, int teamId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var roles = await connection.QueryAsync<string>("SELECT * FROM dbo.ufnGetRoles(@UserId, @TeamId)",
                    new { UserId = userId, TeamId = teamId }, commandType: CommandType.Text);
                return roles.AsList();
            }
            catch (SqlException)
            {
                return new List<string>();
            }
        }

    }
}

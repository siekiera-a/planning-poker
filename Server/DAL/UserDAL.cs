using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Server.Models;
using Server.Services.DataAccess;
using Server.Utils;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Models.Dapper;


namespace Server.DAL
{
    public class UserDAL
    {

        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<UserDAL> _logger;

        public UserDAL(IConnectionFactory connectionFactory, ILogger<UserDAL> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Optional<User>> Save(string username, string email, byte[] password)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                try
                {
                    var user = await connection.QueryFirstAsync<User>("dbo.spUser_CreateUser",
                        new { Name = username, Email = email, Password = password }, commandType: CommandType.StoredProcedure);

                    return Optional<User>.of(user);
                }
                catch (SqlException)
                {
                    return Optional<User>.Empty();
                }
            }
        }

        public async Task<Optional<UserWithPassword>> GetUserByEmail(string email)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                try
                {
                    var user = await connection.QueryFirstOrDefaultAsync<UserWithPassword>("dbo.spUser_GetUserByEmail", new { Email = email },
                        commandType: CommandType.StoredProcedure);

                    return Optional<UserWithPassword>.ofNullable(user);
                }
                catch (SqlException e)
                {
                    _logger.LogCritical(e.Message);
                    return Optional<UserWithPassword>.Empty();
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Server.Services.DataAccess
{
	public class ConnectionFactory : IConnectionFactory
	{

		private readonly IConfiguration _config;
		private readonly string _connectionString;

		public ConnectionFactory(IConfiguration config)
		{
			_config = config;
			_connectionString = "SqlServerConnection";
		}


		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_config.GetConnectionString(_connectionString));
		}
	}
}

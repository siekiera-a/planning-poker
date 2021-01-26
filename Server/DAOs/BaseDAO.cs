using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Services.DataAccess;

namespace Server.DAOs
{
    public abstract class BaseDAO
    {

        protected readonly IConnectionFactory _connectionFactory;

        protected BaseDAO(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

    }
}

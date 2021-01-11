using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Services.DataAccess;

namespace Server.DAL
{
    public abstract class BaseDAL
    {

        protected readonly IConnectionFactory _connectionFactory;

        protected BaseDAL(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

    }
}

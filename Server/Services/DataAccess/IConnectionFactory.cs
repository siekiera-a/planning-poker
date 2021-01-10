﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.DataAccess
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

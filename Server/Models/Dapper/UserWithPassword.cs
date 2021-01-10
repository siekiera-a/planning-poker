using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dapper
{
    public class UserWithPassword : User
    {

        public byte[] Password { get; set; }

    }
}

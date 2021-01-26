using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Dtos.Incoming
{
    public class ChangeRoleRequest : UserIdRequest
    {

        public int Role { get; set; }

    }
}

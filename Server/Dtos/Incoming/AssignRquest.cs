using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Dtos.Incoming
{
    public class AssignRquest : UserIdRequest
    {

        public int TaskId { get; set; }
        public Int16 EstimatedTime { get; set; }

    }
}

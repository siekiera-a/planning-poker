using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Dtos.Incoming
{
    public class TeamRequest : IdRequest
    {
        public string Name { get; set; }
    }
}

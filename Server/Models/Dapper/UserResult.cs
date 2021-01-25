using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dapper
{
    public class UserResult
    {

        public string Description { get; set; }
        public Int16 EstimatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TeamName { get; set; }
        public int TeamId { get; set; }

    }
}

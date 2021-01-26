using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dapper
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Int16 EstimatedTime { get; set; }
    }
}

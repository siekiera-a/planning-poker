using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dapper
{
    public class MeetingDetails
    {

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int OrganizerId { get; set; }
        public string OrganizerName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }

    }
}

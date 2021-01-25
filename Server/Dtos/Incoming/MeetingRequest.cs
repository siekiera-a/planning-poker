using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Dtos.Incoming
{
    public class MeetingRequest : DateTimeRequest
    {

        public List<string> Tasks { get; set; }
        public List<int> InvitedUsers { get; set; }

    }
}
